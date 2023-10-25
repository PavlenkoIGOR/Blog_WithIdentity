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
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public BlogController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }
       

        #region ShowUsers
        //[Authorize]
        //[Authorize(Roles = "Administrator")] //так авторизация работает только для пользователей у которых  Role == "admin"
        [HttpGet]
        public IActionResult ShowUsers()
        {
            //if (user != null)
            //{
            //    FormsAuthentication.SetCookie(model.Name, true); //добавить using System.Web.Security
            //    return Redirection("Index", "Home");
            //}

            var users = _userManager.Users.Select(u => new UsersViewModel()
            {
                Id = u.Id,
                Name = u.UserName,
                Age = u.Age,
                Email = u.Email,
                RoleType = _userManager.GetRolesAsync(u).Result.FirstOrDefault()
            }).ToList();
            //var data2 = _context.Roles.ToList();
            //var roles = from item1 in data1
            //            join item2 in data2 on item1.RoleId equals item2.Id
            //            select item1.Role.RoleType;
            //ViewBag.Roles = roles;
            return View(users);
        }

        [HttpPost]
        public IActionResult ShowUsers(string selectedRole)
        {
            //var data = _context.Users.ToList();
            return View(/*data*/);
        }
        #endregion

        [HttpGet]
        public IActionResult UserPosts()
        {

            return View(new UserPostsViewModel());
        }
        [HttpPost]
        public IActionResult AddUserPosts(UserPostsViewModel viewModel)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            //Необходимая логика обработки текста из Textarea
            string postContent = viewModel.Text;
            Post post = new Post()
            {
                //Name = viewModel.Name, //название статьи
                PublicationDate = DateTime.UtcNow,
                Text = postContent,
                UserId = userId,
                CommentId = "1",
                Tegs=viewModel.Tegs,
            };
            _context.Posts.AddAsync(post);
            _context.SaveChanges();

            if (_signInManager.IsSignedIn(User))
            {
                string logFile1 = Path.Combine(_env.ContentRootPath, "Logs", "UserPostsLogs.txt");
                using (StreamWriter sw = new StreamWriter(logFile1, true))
                {
                    sw.WriteLineAsync($"signInManager сработал");
                    sw.Close();
                }
                // Получаем данные текущего аутентифицированного пользователя

                if (ModelState.IsValid)
                {
                    using (StreamWriter sw = new StreamWriter(logFile1, true))
                    {
                        sw.WriteLineAsync($"{viewModel.Name} Валидна. {viewModel.PublicationDate}. ");
                        sw.Close();
                    }

                    // Верните результат или выполните другие действия
                    return RedirectToAction("UserPosts", "Blog");
                }
            }

                
            return RedirectToAction("UserPosts", "Blog");
        }

        public async Task<IActionResult> EditUser()
        {

            return RedirectToAction("ShowUsers", "Blog");
        }
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
                    fs.WriteLineAsync($"{DateTime.UtcNow} Пользователь удалён! Почта: {user.Email}, пароль {user.PasswordHash}");
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

        //[Authorize]
        [HttpGet]
        public ActionResult ForAuthUsersOnly()
        {
            return Content("Аутентифицирован!");
        }


        [HttpPost]
        public IActionResult PublicatePost(UserPostsViewModel viewModel)
        {

            return Content("Опубликовано!");
        }
    }
}
