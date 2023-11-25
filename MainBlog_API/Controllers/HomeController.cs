using MainBlog.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace MainBlog.Controllers
{

    public class HController : HomeController
    {
        public HController(ILogger logger, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment) : base(logger, userManager, signInManager, environment)
        {
        }
    }
}