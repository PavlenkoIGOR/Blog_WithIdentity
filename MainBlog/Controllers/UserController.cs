using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace MainBlog.Controllers
{
    public class UserController : Controller
    {
        UserManager<Models.User> _userManager;
        SignInManager<Models.User> _signInManager;
        IWebHostEnvironment _env;
        MainBlogDBContext _context;
        public UserController(MainBlogDBContext blogDBContext, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> EditUserPage(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            var role = await _userManager.GetRolesAsync(user);
            if (user == null)
            {
                return StatusCode(404);
            }
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.Id = id;
            viewModel.Email = user.Email;
            viewModel.Name = user.UserName;
            viewModel.Age = user.Age;
            viewModel.RoleType = role.FirstOrDefault();
                
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserByAdmin(string id)//UsersViewModel uVM)
        {
            Models.User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest("Пользователь не найден!");
            }
            UsersViewModel model = new UsersViewModel();
            model.Email = user.Email;
            model.Name = user.UserName;
            return View("EditUser", model);
        }
    }
}
