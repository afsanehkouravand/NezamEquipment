using System.Collections.Generic;
using System.Web.Routing;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.ToolBox
{
    public class ToolBoxOption
    {
        public ToolBoxOption()
        {
            Items = new List<ToolBoxItem>();
        }

        internal RequestContext RequestContext { get; set; }

        internal List<ToolBoxItem> Items { get; set; }

        internal bool EnabledHaveAccessTo { get; set; }
        internal object HaveAccessTo { get; set; }
        internal bool HaveAccessToIsAdmin { get; set; }
        internal IList<RoleAccessDto> HaveAccessToRoleAccess { get; set; }

        public string Style { get; set; }
    }
}