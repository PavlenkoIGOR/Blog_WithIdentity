using Microsoft.AspNetCore.Identity;
using MainBlog.Data.Models;
using MainBlog.Data.Data;

namespace MainBlog.Controllers
{

    public class BController : BlogController
    {
        public BController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, ILogger logger) : base(blogDBContext, userManager, signInManager, environment, logger)
        {
        }
    }
}
