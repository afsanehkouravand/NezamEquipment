using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Framework.Helper.Enum
{
    public enum DatePickerType
    {
        [Display(Name = "شمسی")]
        Shamsi,
        [Display(Name = "میلادی")]
        Miladi,
    }
}