﻿using MainBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MainBlog.Data.Models;
using MainBlog.Data.Data;

namespace MainBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAPIController : Controller
    {
        private MainBlogDBContext _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public BlogAPIController(MainBlogDBContext blogDBContext, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _context = blogDBContext;
        }


		#region ShowUsers Настроено!

		//[Authorize(Roles = "Administrator")] //так авторизация работает только для пользователей у которых  Role == "admin". На данный момент настроено через проверку в View Index
        [HttpGet("ShowUsers")]
        public IActionResult ShowUsers()
        {
            if (User.IsInRole("Administrator"))
            {
                var users = _userManager.Users.Select(u => new UsersViewModel()
                {
                    Id = u.Id,
                    Name = u.UserName,
                    Age = u.Age,
                    Email = u.Email,
                    RoleType = _userManager.GetRolesAsync(u).Result.FirstOrDefault()
                }).ToList();
                return Ok(users);
            }
            return Ok("Перенаправление на страницу Index");
        }

        [HttpPost("ShowUsers")]
        public IActionResult ShowUsers(string selectedRole)
        {
            return StatusCode(200);
        }
        #endregion


        #region EditUser
        [Route("EditUser")]
        [HttpGet]
		public IActionResult EditUser(string userRole)
        {
            var user = User;
            return Ok("перенаправление на страницу ShowUsers");
        }
		#endregion


		#region DeleteUser
		[HttpGet("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return Content("user не найден");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {               
                // Обработка успешного удаления пользователя
                return Ok("перенаправление на страницу ShowUsers"); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!сделать без обновления страницы!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            else
            {
                // Обработка ошибок при удалении пользователя
                var errors = result.Errors;
                return Content("Error");
            }
        }
		#endregion

		#region ShowAllPosts
		[HttpGet("AllPostsPage")]
        public async Task<IActionResult> AllPostsPage()
        {
            HashSet<Teg> tegsModel = _context.Tegs.ToHashSet();
            HashSet<string> tegForView = new HashSet<string>();
            foreach (var tegsItem in tegsModel)
            {
                tegForView.Add(tegsItem.TegTitle);
            }

            var post = await _context.Posts.Include(t=>t.Tegs).Select(p => new AllPostsViewModel {
                Id = p.Id,
                Author = p.User.UserName,
                PublicationTime = p.PublicationDate,
                Title = p.Title,
                Text = p.Text,
                TegsList = tegForView
            }).ToListAsync();
            ViewBag.List = tegForView;            
            return Ok(post);
        }

        [HttpPost("AllPostsPage")]
        public IActionResult AllPostsPage(string selectedRole)
        {
            var data = _context.Users.ToList();
            return StatusCode(200);
        }
        #endregion

        #region показ статей с определенным тегом
        [HttpGet("ShowPostsByTeg")]
        public async Task<IActionResult> ShowPostsByTeg(string tegTitle)
        {
            var tegsModel = await _context.Tegs.Where(t => t.TegTitle == tegTitle).Include(p => p.Posts).ToListAsync();
            HashSet<string> tegForView = new HashSet<string>();
            foreach (var tegsItem in tegsModel)
            {
                tegForView.Add(tegsItem.TegTitle);
            }

            var posts = await _context.Posts
                .Where(p => p.Tegs.Any(t => t.TegTitle == tegTitle))
                .Include(p => p.User)
                .ToListAsync();

            var allPostsViewModels = posts.Select(p => new AllPostsViewModel
            {
                Id = p.Id,
                Author = p.User.UserName,
                PublicationTime = p.PublicationDate,
                Title = p.Title,
                Text = p.Text,
                TegsList = tegForView
            });

            ViewBag.List = tegForView;

            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.            

            return Ok(allPostsViewModels);
        }
		#endregion


		[HttpPost("PublicatePost")]
        public IActionResult PublicatePost(UserBlogViewModel viewModel)
        {

            return Content("Опубликовано!");
        }
    }
}
