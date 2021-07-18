using System.Collections.Generic;
using System.Linq;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Security
{
    internal static class HaveAccessToExtension
    {
        internal static bool Check(this IList<RoleAccessDto> data, bool isAdmin, string area, string controller, string action)
        {
            if (data == null)
                return false;
            
            if (isAdmin)
                return true;
            
            var allAccess = data.Where(x => x.Area == area && x.Controller == controller && x.Action == action).ToList();
            if (!allAccess.Any())
                return false;

            if (allAccess.Any(x => x.HaveAccess == false))
                return false;

            return true;
        }

        internal static bool Check(this object haveAccessTo, string area, string controller, string action)
        {
            var haveAccessToType = haveAccessTo.GetType();

            var propertyInfoIsAdmin = haveAccessToType.GetProperty("IsAdmin");
            if (propertyInfoIsAdmin != null)
            {
                var isAdmin = propertyInfoIsAdmin.GetValue(haveAccessTo, null);
                if (bool.Parse(isAdmin.ToString()))
                    return true;
            }

            var propertyInfoRoleAccess = haveAccessToType.GetProperty("RoleAccess");
            if (propertyInfoRoleAccess != null)
            {
                var roleAccess = propertyInfoRoleAccess.GetValue(haveAccessTo, null);
                var cast = roleAccess.CustomCastToRoleAccess();
                return cast.Check(area, controller, action);
            }

            return false;
        }

    }
}
