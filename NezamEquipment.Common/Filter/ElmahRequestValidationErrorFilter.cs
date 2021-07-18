using System.Web;
using System.Web.Mvc;
using Elmah;

namespace NezamEquipment.Common.Filter
{
    public class ElmahRequestValidationErrorFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpRequestValidationException)
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(context.Exception));
        }
    }
}
