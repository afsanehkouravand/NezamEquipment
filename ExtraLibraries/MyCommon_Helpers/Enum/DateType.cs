using System.ComponentModel.DataAnnotations;

namespace MyCommon.Helpers.Enum
{
    public enum DatePickerType
    {
        [Display(Name = "شمسی")]
        Shamsi,
        [Display(Name = "میلادی")]
        Miladi,
    }
}