using MainBlog.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controllers
{
    public class CommentController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<Models.User> _userManager;
        private SignInManager<Models.User> _signInManager;
        private IWebHostEnvironment _env;
        public CommentController(UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, IWebHostEnvironment environment, MainBlogDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = context;
        }
    }
}
