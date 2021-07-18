using System.ComponentModel.DataAnnotations;

namespace MyCommon.Helpers.Form
{
    public enum EFormMethodType
    {
        [Display(Name = "get")]
        Get,
        [Display(Name = "post")]
        Post,
    }
}