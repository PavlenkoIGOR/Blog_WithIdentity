using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace MainBlog.Controllers
{
    public class PostsController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<Models.User> _userManager;
        private SignInManager<Models.User> _signInManager;
        private IWebHostEnvironment _env;
        public PostsController(MainBlogDBContext blogDBContext, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> UserBlog()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            UserBlogViewModel model = new UserBlogViewModel();
            model.UserPosts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
            model.Id = userId;//Guid.Parse(userId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UserBlog(UserBlogViewModel viewModel)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            //var user = User;
            //var infoAboutUser = await _userManager.GetUserAsync(user);
            //Необходимая логика обработки текста из Textarea
            string postContent = viewModel.Text;
            Post post = new Post()
            {
                //Name = viewModel.Name, //название статьи
                Title = viewModel.Title,
                PublicationDate = DateTime.UtcNow,
                Text = postContent,
                UserId = userId!,
                Tegs = viewModel.HasWritingTags()
            };
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserBlog", "Posts");
            //if (_signInManager.IsSignedIn(User))
            //{
            //    string logFile1 = Path.Combine(_env.ContentRootPath, "Logs", "UserPostsLogs.txt");
            //    using (StreamWriter sw = new StreamWriter(logFile1, true))
            //    {
            //        await sw.WriteLineAsync($"signInManager сработал");
            //        sw.Close();
            //    }
            //    // Получаем данные текущего аутентифицированного пользователя

            //    if (ModelState.IsValid)
            //    {
            //        using (StreamWriter sw = new StreamWriter(logFile1, true))
            //        {
            //            await sw.WriteLineAsync($"{viewModel.Name} Валидна. {viewModel.PublicationDate}. ");
            //            sw.Close();
            //        }

            //        // Верните результат или выполните другие действия
            //        return RedirectToAction("UserPosts", "Posts");
            //    }
            //}


            //return RedirectToAction("UserPosts", "Posts");
        }

        [HttpGet]
        public async Task<IActionResult> PostDiscussion(int id)
        {
            List<Comment> comments = await _context.Comments
                .Include(u => u.User)
                //.ThenInclude(ui => ui.UserIdentity)
                //.Include(p => p.Post)
                .ToListAsync();
            List<Post> posts = await _context.Posts.ToListAsync();

            //List<Teg> tegs = await _context.Tegs.Join(posts, t=>t.Id, p => p.Comments.Where(c=>c. )).ToListAsync();

            var post = await _context.Posts.Include(t => t.Tegs).FirstOrDefaultAsync(i => i.Id == id);
            PostViewModel pVM = new PostViewModel() 
            {
                Id = id,
                CommentsOfPost = post.Comments,
                Title = post.Title,
                AuthorOfPost = post.Title,
                Text = post.Text,
                Tegs = post.Tegs
            };    
            CommentViewModel cVM = new CommentViewModel();
            DiscussionPostViewModel dpVM = new DiscussionPostViewModel { PostVM = pVM, CommentVM = comments };
            if (dpVM == null)
            {
                return NotFound();
            }
            return View("PostDiscussion", dpVM);
        }

        [HttpPost]
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

            return RedirectToAction("AllPostsPage", "Blog");
        }

    }
}


