using MainBlog.Data.Data;
using MainBlog.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace MainBlog.Controllers
{
    public class CController : CommentController
    {
        public CController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, MainBlogDBContext context) : base(userManager, signInManager, environment, context)
        {
        }
    }
}
