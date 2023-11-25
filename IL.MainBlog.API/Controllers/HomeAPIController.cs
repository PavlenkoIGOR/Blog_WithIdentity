using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IL.MainBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeAPIController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {     
            return Ok("Home страница");
        }
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return Ok("приватная страница");
        }
    }
}
