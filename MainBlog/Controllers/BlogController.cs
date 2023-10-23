using Blog;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace MainBlog.Controllers
{
    public class BlogController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _webHostEnvironment;
        public BlogController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = environment;
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
            return View("UserPosts");
        }

        public async Task<IActionResult> EditUser()
        {

            return RedirectToAction("ShowUsers", "Home");
        }
    }
}
