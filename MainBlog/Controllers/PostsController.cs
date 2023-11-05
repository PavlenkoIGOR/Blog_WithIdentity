﻿using MainBlog.Data;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace MainBlog.Controllers
{
    public class PostsController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public PostsController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> UserBlog()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            UserBlogViewModel model = new UserBlogViewModel();
            model.UserPosts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
            model.Id = userId;
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UserBlog(UserBlogViewModel viewModel)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            string postContent = viewModel.Text;
            Post post = new Post()
            {
                //Name = viewModel.Name, //название статьи
                Title = viewModel.Title,
                PublicationDate = DateTime.UtcNow,
                Text = postContent,
                UserId = userId!,
                Tegs = viewModel.HasWritingTags()
            };
            var postExist = await _context.Posts.FirstOrDefaultAsync(p => p.Id == viewModel.PostId);
            if (postExist == null)
            {
                await _context.Posts.AddAsync(post);
            }
            else 
            {
                post.Id = viewModel.PostId;
                _context.Entry(postExist).CurrentValues.SetValues(post);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("UserBlog", "Posts");
        }

        [HttpGet]
        public async Task<IActionResult> PostDiscussion(int id)
        {
            List<Comment> comments = await _context.Comments
                .Include(u => u.User)
                //.ThenInclude(ui => ui.UserIdentity)
                //.Include(p => p.Post)
                .ToListAsync();
            List<Post> posts = await _context.Posts.ToListAsync();

            //List<Teg> tegs = await _context.Tegs.Join(posts, t=>t.Id, p => p.Comments.Where(c=>c. )).ToListAsync();

            var post = await _context.Posts.Include(t => t.Tegs).FirstOrDefaultAsync(i => i.Id == id);
            PostViewModel pVM = new PostViewModel()
            {
                Id = id,
                CommentsOfPost = post.Comments,
                Title = post.Title,
                AuthorOfPost = post.User.UserName,
                Text = post.Text,
                Tegs = post.Tegs
            };
            CommentViewModel cVM = new CommentViewModel();
            DiscussionPostViewModel dpVM = new DiscussionPostViewModel { PostVM = pVM, CommentVM = comments };
            if (dpVM == null)
            {
                return NotFound();
            }
            return View("PostDiscussion", dpVM);
        }

        [HttpPost]
        public async Task<IActionResult> SetComment(DiscussionPostViewModel cVM)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            //var comment = _context.Comments.Where(d => d.PostId == discussionPVM.Id).Select(d=>d).FirstOrDefaultAsync();
            Comment comment = new Comment()
            {
                UserId = userId!,
                CommentText = cVM.CommentText,
                PostId = cVM.PostVM.Id,
                CommentPublicationTime = DateTime.UtcNow
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllPostsPage", "Blog");
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int postId)
        {
            Post? post = await _context.Posts.Include(u => u.Tegs).FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            { return BadRequest(); }

            UserBlogViewModel ubVM = new UserBlogViewModel()
            {
                PostId = postId,
                Title = post.Title,
                Text = post.Text,
                PublicationDate = post.PublicationDate,
                tegsList = post.Tegs,
                tegs = string.Join(", ", post.Tegs.Select(t => t.TegTitle))
            };

            return View("EditPost", ubVM);
        }
        [HttpGet]
        public async Task<IActionResult> EditPostByAdminModer(UserBlogViewModel viewModel)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

            string postContent = viewModel.Text;
            Post post = new Post()
            {
                Title = viewModel.Title,
                PublicationDate = DateTime.UtcNow,
                Text = postContent,
                UserId = userId!,
                Tegs = viewModel.HasWritingTags()
            };

            var postExist = await _context.Posts.FirstOrDefaultAsync(p => p.Id == viewModel.PostId);
            if (postExist == null)
            {
                await _context.Posts.AddAsync(post);
            }
            else
            {
                post.Id = viewModel.PostId;
                _context.Entry(postExist).CurrentValues.SetValues(post);
            }
            await _context.SaveChangesAsync();
                        
            Post? post1 = await _context.Posts.Include(u => u.Tegs).FirstOrDefaultAsync(p => p.Id == viewModel.PostId);
            if (post == null)
            { return BadRequest(); }
            UserBlogViewModel ubVM = new UserBlogViewModel()
            {
                PostId = viewModel.PostId,
                Title = post1.Title,
                Text = post1.Text,
                PublicationDate = post1.PublicationDate,
                tegsList = post1.Tegs,
                tegs = string.Join(", ", post1.Tegs.Select(t => t.TegTitle))
            };
            
            return View("EditPost", ubVM);
        }

        #region Удаление статьи администратором
        [HttpPost]
        public async Task<IActionResult> DeletePostByAdmin(int postId)
        {
            Post postfodDelete = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (postfodDelete == null) {  return BadRequest(); }
            else
            {
                _context.Posts.Remove(postfodDelete);
                _context.SaveChanges();
            }

            return RedirectToAction("AllPostsPage", "Blog");
        }
        #endregion
    }
}
