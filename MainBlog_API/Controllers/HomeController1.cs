using Microsoft.AspNetCore.Mvc;

namespace MainBlog_API.Controllers
{
    public class HomeController1 : ControllerBase
    {
        /// <summary>
        /// "nj ckdfchl;kgdgfkl;
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(1);
        }
    }
}
