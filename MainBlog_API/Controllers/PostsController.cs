using MainBlog.Data.Data;
using MainBlog.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace MainBlog.Controllers
{
    public class PController : PostsController
    {
        public PController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, ILogger logger) : base(blogDBContext, userManager, signInManager, environment, logger)
        {
        }
    }
}
