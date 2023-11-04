using Blog;
using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;

namespace MainBlog.Controllers
{
    public class BlogController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<Models.User> _userManager;
        private SignInManager<Models.User> _signInManager;
        private IWebHostEnvironment _env;
        public BlogController(MainBlogDBContext blogDBContext, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }


        #region ShowUsers Настроено!
        //[Authorize]
        //[Authorize(Roles = "Administrator")] //так авторизация работает только для пользователей у которых  Role == "admin". На данный момент настроено через проверку в View Index
        [HttpGet]
        public IActionResult ShowUsers()
        {
            if (User.IsInRole("Administrator"))
            {
                var users = _userManager.Users.Select(u => new UsersViewModel()
                {
                    Id = u.Id,
                    Name = u.UserName,
                    Age = u.Age,
                    Email = u.Email,
                    RoleType = _userManager.GetRolesAsync(u).Result.FirstOrDefault()
                }).ToList();
                return View(users);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ShowUsers(string selectedRole)
        {
            //var data = _context.Users.ToList();
            return View(/*data*/);
        }
        #endregion


        #region EditUser
        public async Task<IActionResult> EditUser(string userRole)
        {
            //userRole;
                var user = User;
          

            return RedirectToAction("ShowUsers", "Blog");
        }
        #endregion

        
        #region DeleteUser
        [HttpGet]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return Content("user не найден");                
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                string filePath = Path.Combine(_env.ContentRootPath, "Logs", "DeleteUsersLogs.txt");
                using (StreamWriter fs = new StreamWriter(filePath, true))
                {
                    await fs.WriteLineAsync($"{DateTime.UtcNow} Пользователь удалён! Почта: {user.Email}, пароль {user.PasswordHash}");
                    fs.Close();
                }
                // Обработка успешного удаления пользователя
                return RedirectToAction("ShowUsers", "Blog"); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!сделать без обновления страницы!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            else
            {
                // Обработка ошибок при удалении пользователя
                var errors = result.Errors;
                // ...

                return Content("Error");
            }
            return RedirectToAction("ShowUsers");
        }
        #endregion


        #region ShowAllPosts
        [HttpGet]
        public async Task<IActionResult> AllPostsPage()
        {
            var post = _context.Posts.Select(p => new AllPostsViewModel {
                Id = p.Id,
                Author = p.User.UserName,
                PublicationTime = p.PublicationDate,
                Title = p.Title,
                Text = p.Text
            });
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> AllPostsPage(string selectedRole)
        {
            //var data = _context.Users.ToList();
            return View(/*data*/);
        }
        #endregion



        //[Authorize]
        [HttpGet]
        public ActionResult ForAuthUsersOnly()
        {
            return Content("Аутентифицирован!");
        }


        [HttpPost]
        public IActionResult PublicatePost(UserBlogViewModel viewModel)
        {

            return Content("Опубликовано!");
        }
    }
}
