using MainBlog.BL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace MainBlog.Controllers
{
    public class ErrorPagesController : Controller
    {
        private readonly ILogger<ErrorPagesController> _logger;
        IWebHostEnvironment _env;
        public ErrorPagesController(ILogger<ErrorPagesController> logger, IWebHostEnvironment environment)
        { 
            _logger = logger;
            _env = environment;
        }
        public async Task<IActionResult> MyErrorsAction(int statusCode) 
        {
            switch (statusCode)
            {
                case 404:
                    _logger.LogInformation($"{DateTime.UtcNow}: возникла ошибка {statusCode}.");
                    await WriteActions.CreateLogFolder_File(_env, "Errors", "Возникла ошибка 404");
                    return View("PagenotFound");
                case 400:
					_logger.LogInformation($"{DateTime.UtcNow}: возникла ошибка {statusCode}.");
                    await WriteActions.CreateLogFolder_File(_env, "Errors", $"Возникла ошибка 400");
                    return View("400");
                case 401:
					_logger.LogInformation($"{DateTime.UtcNow}: возникла ошибка {statusCode}.");
                    await WriteActions.CreateLogFolder_File(_env, "Errors", $"Возникла ошибка 401");
                    return View("401");
            }
            _logger.LogInformation($"{statusCode}");
            await WriteActions.CreateLogFolder_File(_env, "Errors", $"Возникла ошибка 500");
            return View("500");
        }
    }
}

