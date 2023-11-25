using Microsoft.AspNetCore.Identity;
using MainBlog.Data.Models;

namespace MainBlog.Controllers
{

    public class ARController : AuthRegController
    {
        public ARController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, ILogger logger) : base(userManager, signInManager, environment, logger)
        {
        }
    }
}


