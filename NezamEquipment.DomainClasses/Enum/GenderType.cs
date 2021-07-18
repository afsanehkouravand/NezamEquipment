using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male,

        [Display(Name = "زن")]
        Female,
    }
}