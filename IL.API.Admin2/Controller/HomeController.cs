using Microsoft.AspNetCore.Mvc;

namespace IL.API.Admin2.Controller;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult HomeControllersdffd()
    {
        return Ok(1);
    }
}
