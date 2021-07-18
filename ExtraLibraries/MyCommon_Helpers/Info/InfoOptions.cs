using System.Collections.Generic;
using System.Web.Routing;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Info
{
    public class InfoOption
    {
        public InfoOption()
        {
            Colume = EInfoColumeType.Column3;
            Title = "اطلاعات";
            Color = EInfoColorType.Info;
            InfoItems = new List<InfoItem>();
            PanelFooterButtons = new List<InfoButton>();
        }

        internal RequestContext RequestContext { get; set; }

        internal string PanelClass { get; set; }

        internal bool EnabledHaveAccessTo { get; set; }
        internal object HaveAccessTo { get; set; }
        internal bool HaveAccessToIsAdmin { get; set; }
        internal IList<RoleAccessDto> HaveAccessToRoleAccess { get; set; }

        internal string Title { get; set; }

        internal EInfoColumeType Colume { get; set; }

        internal EInfoColorType Color { get; set; }

        internal List<InfoItem> InfoItems { get; set; }

        internal bool EnablePanelFooter { get; set; }
        internal List<InfoButton> PanelFooterButtons { get; set; }

        internal string Labelclass { get; set; }
    }
}