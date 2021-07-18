using System.Collections.Generic;
using System.Web.Routing;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Ul
{
    public class UlOption
    {
        internal UlOption()
        {
            Items = new List<UlItem>();
        }

        internal RequestContext RequestContext { get; set; }

        internal List<UlItem> Items { get; set; }

        internal bool EnabledHaveAccessTo { get; set; }
        internal object HaveAccessTo { get; set; }
        internal bool HaveAccessToIsAdmin { get; set; }
        internal IList<RoleAccessDto> HaveAccessToRoleAccess { get; set; }
    }
}