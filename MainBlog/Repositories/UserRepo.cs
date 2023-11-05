using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog;
using MainBlog;
using MainBlog.Controllers;
using MainBlog.Data;
using MainBlog.Models;
using Microsoft.AspNetCore.Identity;

namespace MainBlog.BL.Repositories
{
    public interface IUserRepo
    {

    }

    internal class UserRepo : IUserRepo
    {
        ILogger<HomeController> _logger;
        MainBlogDBContext _context;
        UserManager<User> _userManager;
        public UserRepo(ILogger<HomeController> logger, MainBlogDBContext contextDB, UserManager<User> userManager)
        {
            _context = contextDB;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<User> GetUserById(string Id)
        {
            User? userById = await _userManager.FindByIdAsync(Id);
            
            return userById;
        }
    }
}
