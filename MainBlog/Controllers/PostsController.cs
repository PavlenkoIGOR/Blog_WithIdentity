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
        public IActionResult UserPosts()
        {

            return View(new UserPostsViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> AddUserPosts(UserPostsViewModel viewModel)
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
                PublicationDate = DateTime.UtcNow,
                Text = postContent,
                UserId = userId!,
                CommentId = "1",
                Tegs = new List<Teg>() { new Teg() { Name = "Name1" }, new Teg() { Name = "Name2" } }
            };
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserPosts", "Posts");
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
    }
}
