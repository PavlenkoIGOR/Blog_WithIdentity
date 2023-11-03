using MainBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MainBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<Models.User> _userManager;
        private SignInManager<Models.User> _signInManager;
        private IWebHostEnvironment _env;
        //private readonly Imylog _imylog;

        public HomeController(ILogger<HomeController> logger, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            //_imylog = imylog;
        }

        public IActionResult Index()
        {
                return View();
        }
        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            _logger.LogInformation("Nen ldfgdfg");
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