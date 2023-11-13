using MainBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MainBlog.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        //private readonly Imylog _imylog;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
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

  
        //[Authorize(Roles = "Administrator")]//(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)//]
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

        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorMessage = exception?.Error?.Message;

            // Другие действия для обработки ошибки

            return View("404", errorMessage);
        }

    }
}