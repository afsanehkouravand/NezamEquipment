using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Framework.Helper.Enum
{
    public enum DatePickerPlacement
    {
        [Display(Name = "بالا")]
        Top,
        [Display(Name = "راست")]
        Right,
        [Display(Name = "پایین")]
        Bottom,
        [Display(Name = "چپ")]
        Left,
    }
}