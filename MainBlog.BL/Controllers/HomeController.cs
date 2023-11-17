using MainBlog.Data.Models;
using MainBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MainBlog.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;

        public HomeController(ILogger logger, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
        }

        
        public IActionResult Index()
        {
            //try
            //{
                int a = 5;
                int b = 0;
                var c = a / b;

            //}
            //catch (Exception ex)
            //{
            //    _logger.WriteError(ex.Message);
            //}
            return View();
        }
        
        public IActionResult Privacy()
        {
            _logger.WriteEvent("Nen ldfgdfg");
            return View();
        }

        [HttpGet]
        public IActionResult GreetingPage()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}