using System.Web.Mvc;

namespace NezamEquipment.Common.Filter
{
   public class RemoveServerHeaderFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            // for prevent attack
            response.Headers.Remove("Server");
            base.OnActionExecuting(filterContext);
        }
    }
}
