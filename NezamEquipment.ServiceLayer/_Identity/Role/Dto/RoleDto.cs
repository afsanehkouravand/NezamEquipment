using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Identity.Enum;

namespace NezamEquipment.ServiceLayer._Identity.Role.Dto
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        
        [Display(Name = "بخش")]
        public AreaType AreaType { get; set; }

        [Display(Name = "نقش پیش فرض")]
        public bool IsDefaultRole { get; set; }

        [Display(Name = "فروشگاه")]
        public Guid? StoreId { get; set; }
    }
}
