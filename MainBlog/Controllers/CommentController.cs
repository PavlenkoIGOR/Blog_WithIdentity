using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost]
        public async Task<IActionResult> SetComment(DiscussionPostViewModel discussionPVM)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            //var comment = _context.Comments.Where(d => d.PostId == discussionPVM.Id).Select(d=>d).FirstOrDefaultAsync();
            Comment comment = new Comment()
            {
                UserId = userId!,
                CommentText = discussionPVM.CommentText,
                PostId = discussionPVM.Id
            };
            await _context.Comments.AddAsync(comment);
            return RedirectToAction("PostDiscussion", "Posts");
        }
    }
}
