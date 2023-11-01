using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //[HttpGet]
        //public async Task<IActionResult> PostDiscussion(DiscussionPostViewModel cVM)
        //{
        //    var post = await _context.Posts.Where(x => x.Id == cVM.PostVM.Id).Select(p => new DiscussionPostViewModel
        //    {
        //        new PostViewModel
        //        {
        //            Id = cVM.PostVM.Id,
        //        AuthorOfPost = p.User.UserName,
        //        PublicationTime = p.PublicationDate,
        //        Title = p.Title,
        //        Text = p.Text,
        //        UsersComments = p.Comments,
        //        }
        //    }).FirstOrDefaultAsync();

        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}
    }
}
