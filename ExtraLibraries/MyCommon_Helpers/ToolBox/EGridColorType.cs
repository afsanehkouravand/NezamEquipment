using System.ComponentModel.DataAnnotations;

namespace MyCommon.Helpers.ToolBox
{
    public enum EToolBoxColorType
    {
        [Display(Name = "default")]
        Default,
        [Display(Name = "success")]
        Success,
        [Display(Name = "warning")]
        Warning,
        [Display(Name = "info")]
        Info,
        [Display(Name = "danger")]
        Danger,
        [Display(Name = "primary")]
        Primary,
    }
}