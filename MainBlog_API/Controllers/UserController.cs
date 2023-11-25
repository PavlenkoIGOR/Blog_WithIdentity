using MainBlog.Data.Data;
using MainBlog.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace MainBlog.Controllers
{
    public class UController : UserController
    {
        public UController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment) : base(blogDBContext, userManager, signInManager, environment)
        {
        }
    }
}
