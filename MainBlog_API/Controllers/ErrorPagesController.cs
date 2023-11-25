namespace MainBlog.Controllers
{
    public class EPController : ErrorPagesController
    {
        public EPController(ILogger<ErrorPagesController> logger, IWebHostEnvironment environment) : base(logger, environment)
        {
        }
    }
}

