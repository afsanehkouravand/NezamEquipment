using System.Collections;
using System.Collections.Generic;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Ul
{
    public class UlDataOption
    {
        private readonly UlOption _option;

        public UlDataOption(UlOption options)
        {
            _option = options;
        }

        public void SetHaveAccessTo(object haveAccessTo = null, bool enabled = true)
        {
            _option.EnabledHaveAccessTo = enabled;
            _option.HaveAccessTo = haveAccessTo;
            _option.HaveAccessToRoleAccess = new List<RoleAccessDto>();

            if (haveAccessTo == null)
                return;

            var haveAccessToType = haveAccessTo.GetType();

            var propertyInfoIsAdmin = haveAccessToType.GetProperty("IsAdmin");
            if (propertyInfoIsAdmin != null)
            {
                var isAdmin = propertyInfoIsAdmin.GetValue(haveAccessTo, null);
                _option.HaveAccessToIsAdmin = isAdmin != null && bool.Parse(isAdmin.ToString());
            }

            var propertyInfoRoleAccess = haveAccessToType.GetProperty("RoleAccess");
            if (propertyInfoRoleAccess != null)
            {
                var roleAccess = propertyInfoRoleAccess.GetValue(haveAccessTo, null);
                if (roleAccess != null)
                {
                    var roleAccessCast = (IList)roleAccess;
                    foreach (var item in roleAccessCast)
                    {
                        _option.HaveAccessToRoleAccess.Add(item.CustomCastToRoleAccess());
                    }
                }
            }

        }

    }
}