using MainBlog.Data.Data;
using MainBlog.Data.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IL.MainBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        IWebHostEnvironment _env;
        MainBlogDBContext _context;
        public UserAPIController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }

        [HttpGet("EditUserPage")]
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

            return Ok(viewModel);
        }
    }
}
