using MainBlog.BL.Services;
using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly IUserService _userService;

        public UserController(MainBlogDBContext blogDBContext, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, IWebHostEnvironment environment, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;

            _userService = userService;
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
        public async Task<IActionResult> EditUserByAdmin(UsersViewModel usersVM)
        {
            if (ModelState.IsValid)
            {
                UsersViewModel uVM = new UsersViewModel();
                User user = await _context.Users.FindAsync(usersVM.Id);
                if (user == null)
                {
                    return BadRequest("Пользователь не найден!");
                }
                else
                {
                    var role = await _userManager.GetRolesAsync(user);

                    user.Id = usersVM.Id;
                    uVM.Id = usersVM.Id;
                    uVM.Email = user.Email;
                    uVM.Name = user.UserName;
                    uVM.Age = usersVM.Age;
                    uVM.RoleType = usersVM.RoleType;

                    user.Age = usersVM.Age;
                    user.Id = usersVM.Id;
                    user.Email = usersVM.Email;
                    user.UserName = usersVM.Name;

                    await _userManager.UpdateAsync(user);
                    await _userManager.AddToRoleAsync(user, usersVM.RoleType);
                }

                IList<string> roles = await _userManager.GetRolesAsync(user);

                return View("EditUserPage", uVM);
            }
            return BadRequest("Пользователь не найден!");
        }
    }
}
