using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controllers
{
    public class ErrorPagesController : Controller
    {
        private readonly ILogger<ErrorPagesController> _logger;
        public ErrorPagesController(ILogger<ErrorPagesController> logger)
        { 
            _logger = logger;
        }
        public IActionResult MyErrorsAction(int statusCode) 
        {
            switch (statusCode)
            {
                case 404:
                    _logger.LogInformation($"{DateTime.UtcNow}: возникла ошибка {statusCode}.");
                    return View("PagenotFound");
                case 400:
					_logger.LogInformation($"{DateTime.UtcNow}: возникла ошибка {statusCode}.");
					return View("400");
                case 403:
					_logger.LogInformation($"{DateTime.UtcNow}: возникла ошибка {statusCode}.");
					return View("403");
            }
            _logger.LogInformation($"{statusCode}");
            return View("Error");
        }
    }
}

