using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controllers
{
    public class ErrorPagesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ErrorPagesController(ILogger<HomeController> logger)
        { 
            _logger = logger;
        }
        public IActionResult MyErrorsAction(int statusCode) 
        {
            switch (statusCode)
            {
                case 404:
                    return View("PagenotFound");
                case 400:
                    return View("400");
                case 403:
                    return View("403");
            }
            
            return View("Error");
        }
    }
}

