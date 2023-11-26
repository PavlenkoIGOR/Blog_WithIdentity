using MainBlog.Data;
using MainBlog.Data.Data;
using MainBlog.Data.Models;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace MainBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsAPIController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        ILogger _logger;
        public PostsAPIController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
            _logger = logger;
        }

        //[Authorize]
        [HttpGet("UserBlog")]
        public async Task<IActionResult> UserBlog()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            UserBlogViewModel model = new UserBlogViewModel();
            model.UserPosts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
            model.Id = userId;

            return Ok(model);
        }
        [HttpPost("UserBlog")]
        public async Task<IActionResult> UserBlog(UserBlogViewModel viewModel)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {

                if (ModelState.IsValid)
                {
                    string postContent = viewModel.Text;

                    // Проверка существующего поста
                    var existingPost = await _context.Posts
                    .Include(p => p.Tegs)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == viewModel.PostId);

                    if (existingPost == null)
                    {
                        // Создание нового поста
                        var newPost = new Post
                        {
                            Title = viewModel.Title,
                            PublicationDate = DateTime.UtcNow,
                            Text = postContent,
                            UserId = userId
                        };

                        var tags = viewModel.HasWritingTags();
                        foreach (var tag in tags)
                        {
                            var existingTag = await _context.Tegs.FirstOrDefaultAsync(t => t.TegTitle == tag.TegTitle);
                            if (existingTag == null)
                            {
                                existingTag = new Teg { TegTitle = tag.TegTitle };
                                _context.Tegs.Add(existingTag);
                            }
                            newPost.Tegs.Add(existingTag); // Добавить тег в пост
                        }

                        _context.Posts.Add(newPost);
                    }
                    await _context.SaveChangesAsync();
                    return Ok("Статейка добавлена!");
                }
                else
                {
                    var addPosstsForView = await _context.Posts
                        .Where(u => u.UserId == userId).ToListAsync();
                    viewModel.UserPosts = addPosstsForView;
                    return Ok(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}");
            }
            return Ok(viewModel);
        }

        [HttpGet("PostDiscussion")]
        public async Task<IActionResult> PostDiscussion(int id)
        {
            List<Comment> comments = await _context.Comments
                .Include(u => u.User)
                .ToListAsync();
            List<Post> posts = await _context.Posts.Include(u => u.User).ToListAsync();
            var post = await _context.Posts.Include(t => t.Tegs).FirstOrDefaultAsync(i => i.Id == id);
            PostViewModel pVM = new PostViewModel()
            {
                Id = id,
                CommentsOfPost = post.Comments,
                Title = post.Title,
                AuthorOfPost = post.User.UserName,
                Text = post.Text,
                Tegs = post.Tegs
            };
            CommentViewModel cVM = new CommentViewModel();
            DiscussionPostViewModel dpVM = new DiscussionPostViewModel { PostVM = pVM, CommentVM = comments };
            if (dpVM == null)
            {
                return NotFound();
            }
            return Ok(dpVM);
        }

        [HttpPost("SetComment")]
        public async Task<IActionResult> SetComment(DiscussionPostViewModel cVM)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            //var comment = _context.Comments.Where(d => d.PostId == discussionPVM.Id).Select(d=>d).FirstOrDefaultAsync();
            Comment comment = new Comment()
            {
                UserId = userId!,
                CommentText = cVM.CommentText,
                PostId = cVM.PostVM.Id,
                CommentPublicationTime = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            

            return Ok("Здесь все посты");
        }

        [HttpGet("EditPost")]
        public async Task<IActionResult> EditPost(int postId)
        {
            var post = await _context.Posts.Include(p => p.Tegs).FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                return BadRequest();
            }

            var ubVM = new UserBlogViewModel
            {
                PostId = postId,
                Title = post.Title,
                Text = post.Text,
                PublicationDate = post.PublicationDate,
                tegsList = post.Tegs,
                tegs = string.Join(", ", post.Tegs.Select(t => t.TegTitle))
            };
            return Ok(ubVM);
        }

        [HttpPost("EditPost")]
        public async Task<IActionResult> EditPost(UserBlogViewModel viewModel)
        {
            var post = await _context.Posts.Include(p => p.Tegs).FirstOrDefaultAsync(p => p.Id == viewModel.PostId);
            if (post == null)
            {
                return BadRequest();
            }
            // Очистить существующие теги
            post.Tegs.Clear();
            // Обновить свойства объекта Post
            post.Title = viewModel.Title;
            post.Text = viewModel.Text;

            // Добавить или обновить выбранные теги
            var tags = viewModel.HasWritingTags(); // Предположим, что HasWritingTags возвращает список объектов Teg
            foreach (var tag in tags)
            {
                var existingTag = await _context.Tegs.FirstOrDefaultAsync(t => t.Id == tag.Id);
                if (existingTag == null)
                {
                    post.Tegs.Add(tag); // Добавить новый тег
                }
                else
                {
                    post.Tegs.Add(existingTag); // Использовать существующий тег
                }
            }

            _context.Posts.Update(post); // Обновить пост в контексте

            await _context.SaveChangesAsync(); // Сохранить изменения
            return Ok(viewModel);
        }

        [HttpGet("EditPostByAdminModer")]
        public async Task<IActionResult> EditPostByAdminModer(UserBlogViewModel viewModel)
        {
            var post = await _context.Posts.Include(p => p.Tegs).FirstOrDefaultAsync(p => p.Id == viewModel.PostId);
            if (post == null)
            {
                return BadRequest();
            }
            // Очистить существующие теги
            post.Tegs.Clear();
            // Обновить свойства объекта Post
            post.Title = viewModel.Title;
            post.Text = viewModel.Text;

            // Добавить или обновить выбранные теги
            var tags = viewModel.HasWritingTags(); // Предположим, что HasWritingTags возвращает список объектов Teg
            foreach (var tag in tags)
            {
                var existingTag = await _context.Tegs.FirstOrDefaultAsync(t => t.Id == tag.Id);
                if (existingTag == null)
                {
                    post.Tegs.Add(tag); // Добавить новый тег
                }
                else
                {
                    post.Tegs.Add(existingTag); // Использовать существующий тег
                }
            }
            _context.Posts.Update(post); // Обновить пост в контексте

            await _context.SaveChangesAsync(); // Сохранить изменения
            return Ok(viewModel);
        }


        #region Удаление статьи администратором
        [HttpPost("DeletePostByAdmin")]
        public async Task<IActionResult> DeletePostByAdmin(int postId)
        {
            Post postfodDelete = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (postfodDelete == null) { return BadRequest(); }
            else
            {
                _context.Posts.Remove(postfodDelete);
                _context.SaveChanges();
            }

            return Ok("Произведено направление на страницу AllPostsPage");
        }
        #endregion
    }
}
