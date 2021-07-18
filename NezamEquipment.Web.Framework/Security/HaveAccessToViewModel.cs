using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;

namespace NezamEquipment.Web.Framework.Security
{
    public static class HaveAccessToExtension
    {
        public static bool Check(this IList<RoleAccessDto> data, string area, string controller, string action)
        {
            var allAccess = data.Where(x => x.Area == area && x.Controller == controller && x.Action == action).ToList();
            if (!allAccess.Any())
                return false;

            if (allAccess.Any(x=>x.HaveAccess == false))
                return false;

            return true;
        }

        public static bool Check(this HaveAccessToViewModel haveAccessTo,string area, string controller, string action)
        {
            if (haveAccessTo.IsAdmin)
                return true;

            return haveAccessTo.RoleAccess.Check(area, controller, action);
        }

        public static bool Check(this HaveAccessToViewModel haveAccessTo, ViewContext viewContext, string action)
        {
            if (haveAccessTo.IsAdmin)
                return true;

            var controller = viewContext.RouteData.Values["controller"].ToString();
            var area = viewContext.RouteData.Values["area"].ToString();

            return haveAccessTo.RoleAccess.Check(area: area, controller: controller, action: action);
        }

        public static bool Check(this HaveAccessToViewModel haveAccessTo, string action)
        {
            if (haveAccessTo.IsAdmin)
                return true;

            var area = haveAccessTo.CurrentArea;
            if (string.IsNullOrWhiteSpace(area))
            {
                throw new Exception("no area name.");
            }

            var controller = haveAccessTo.CurrentController;
            if (string.IsNullOrWhiteSpace(controller))
            {
                throw new Exception("no controller name.");
            }

            return haveAccessTo.RoleAccess.Check(area: area, controller: controller, action: action);
        }

    }
}
