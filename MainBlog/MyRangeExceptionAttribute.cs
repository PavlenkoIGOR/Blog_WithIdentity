using System.Web.Mvc;

///попытка написать свой аттрибут
namespace MainBlog
{
    public class MyRangeExceptionAttribute : FilterAttribute, IExceptionFilter
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
