using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Areas.Admin.Features.Home;
using NezamEquipment.Web.Framework.Attribute;

namespace NezamEquipment.Web.Areas.Admin
{
    [AuthorizeAreaName("کنترل پنل کل", Order = 1)]
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            var namespacesList = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(AdminBaseController).IsAssignableFrom(type) && !type.Name.StartsWith("T4MVC_"))
                .Select(x => x.Namespace).ToList();
            namespacesList.Add("NezamEquipment.Web.Areas.Admin.Features.Account");

            namespacesList.Add(typeof(HomeController).Namespace);

            var namespaces = namespacesList.ToArray();

            context.MapRoute(
                "Admin_Login",
                "Admin/",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: namespaces
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: namespaces
            );
        }
    }
}