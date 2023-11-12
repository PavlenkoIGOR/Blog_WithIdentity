using System.Web.Mvc;

namespace MainBlog
{
    public class RangeExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
                    filterContext.Exception is ArgumentOutOfRangeException)
            {
                filterContext.Result =
                    new RedirectResult("~/ErrorPages/RangeErrorPage.chtml");
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
