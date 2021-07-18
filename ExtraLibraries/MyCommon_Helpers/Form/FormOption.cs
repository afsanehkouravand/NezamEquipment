using System.Collections.Generic;
using System.Web.Routing;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Form
{
    public class FormOption
    {
        public FormOption()
        {
            HiddenProperties = new Dictionary<string, string>();
            DataInfos = new Dictionary<string, string>();
            FormItems = new List<FormItem>();
            FormButtons = new List<FormButton>();
            SubmitColor = EFormColorType.Success;
            SubmitText = "ارسال";
            UseModelValue = true;
            IsSubmit = true;
            SubmitAttributes = new Dictionary<string, string>();
        }

        internal RequestContext RequestContext { get; set; }
        internal bool IsMaterialStyle { get; set; }
        internal bool HideFormWell { get; set; }

        internal string UpdateTarget { get; set; }
        internal string OnSuccess { get; set; }
        internal string OnFailure { get; set; }
        internal string OnComplete { get; set; }
        internal string OnBegin { get; set; }
        internal string Loading { get; set; }

        internal string Url { get; set; }

        internal bool EnabledHaveAccessTo { get; set; }
        internal object HaveAccessTo { get; set; }
        internal bool HaveAccessToIsAdmin { get; set; }
        internal IList<RoleAccessDto> HaveAccessToRoleAccess { get; set; }

        internal string HaveAccessToArea { get; set; }
        internal string HaveAccessToController { get; set; }
        internal string HaveAccessToAction { get; set; }

        internal IDictionary<string, string> HiddenProperties { get; set; }

        internal IList<FormItem> FormItems { get; set; }

        internal IList<FormButton> FormButtons { get; set; }

        internal string GoToPage { get; set; }

        internal EFormMethodType FormMethod { get; set; }

        internal bool IsAjaxForm { get; set; }

        internal bool IsDisabled { get; set; }
        internal bool IsComplex { get; set; }
        internal string Prefix { get; set; }
        internal bool IsSubmit { get; set; }
        internal string SubmitOnClick { get; set; }
        internal string SubmitText { get; set; }
        internal IDictionary<string, string> SubmitAttributes { get; set; }
        internal EFormColorType SubmitColor { get; set; }

        internal IDictionary<string, string> DataInfos { get; set; }

        internal bool UseModelValue { get; set; }

        internal bool InsidePanelDiv { get; set; }
        internal string InsidePanelDivTitle { get; set; }
        internal EFormColorType InsidePanelDivColor { get; set; }

        internal string CssClass { get; set; }
    }
}