using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controllers
{
    public class ErrorPagesController : Controller
    {
        [Route("Errors/{id?}")]
        public async Task<IActionResult> ErrorsRedirect(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                switch (statusCode)
                {
                    case 400: return RedirectToPage("/ErrorPages/ResourceIsNotFoundPage");
                    case 401: return RedirectToPage("/ErrorPages/AccessIsDeniedPage");
                    default: return RedirectToPage("/ErrorPages/SomethingWrongPage");
                }
            }
            return RedirectToPage("/ErrorPages");
        }
        [Route("FuckingError")]
        public IActionResult MakeError()
        {
            return StatusCode(402);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
