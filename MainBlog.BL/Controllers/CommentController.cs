using MainBlog.Data;
using MainBlog.Data.Data;
using MainBlog.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controllers
{
    public class CommentController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public CommentController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, MainBlogDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = context;
        }
    }
}
