using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum NationalityType
    {
        [Display(Name = "ایرانی")]
        Iranian,
        [Display(Name = "غیرایرانی")]
        NoIranian,
    }
}