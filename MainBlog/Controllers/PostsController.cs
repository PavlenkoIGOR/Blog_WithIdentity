using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MainBlog.Controllers
{
    public class PostsController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public PostsController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }

        //[Authorize]
        [HttpGet]
        public IActionResult UserBlog()
        {

            return View(new UserBlogViewModel());
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

        //неработающий оригинал
        //[HttpGet]
        //public async Task<IActionResult> PostDiscussion2(DiscussionPostViewModel cVM)
        //{
        //    //var comment = await _context.Comments.;
        //    var post = await _context.Posts.Where(x => x.Id == cVM.PostId).Select(p => new DiscussionPostViewModel
        //    {
        //        Id = cVM.PostId,
        //        AuthorOfPost = p.User.UserName,
        //        PublicationTime = p.PublicationDate,
        //        Title = p.Title,
        //        Text = p.Text,
        //        UsersComments = p.Comments,
        //    }).FirstOrDefaultAsync();

        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        //оригинал
        //[HttpGet]
        //public async Task<IActionResult> PostDiscussion(int id)
        //{
        //    //var comment = await _context.Comments.Join(_context.Users, c => c.UserId, u => u.Id, (c, u) => { c.User.UserName = u.UserName; });
        //    var post = await _context.Posts.Where(x => x.Id == id).Select(p => new DiscussionPostViewModel
        //    {
        //        Id = id,
        //        AuthorOfPost = p.User.UserName,
        //        PublicationTime = p.PublicationDate,
        //        Title = p.Title,
        //        Text = p.Text,
        //        UsersComments = p.Comments
        //    }).FirstOrDefaultAsync();

        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        [HttpGet]
        public async Task<IActionResult> PostDiscussion(int id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync();
            //string AuthorOfPost = user.Posts.;
            CommentViewModel? CVM = await _context.Comments.Where(c=>c.PostId == id).Select(s=> new CommentViewModel
            {
                CommentText = s.CommentText, Author = "VasyaPupkin", PublicationDate = s.CommentPublicationTime
            }).FirstOrDefaultAsync();
            PostViewModel? PVM = await _context.Posts.Where(x => x.Id == id).Select(p => new PostViewModel
            {
                Id = id,
                AuthorOfPost = p.User.UserName,
                PublicationDateOfPost = p.PublicationDate,
                Title = p.Title,
                Text = p.Text,
                CommentsOfPost = p.Comments
            }).FirstOrDefaultAsync();
            DiscussionPostViewModel dpVM = new DiscussionPostViewModel { PostVM = PVM, CommentVM = CVM };
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
                CommentText = cVM.CommentVM.CommentText,
                PostId = cVM.PostVM.Id,
                CommentPublicationTime = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("PostDiscussion", "Posts");
        }

    }
}


