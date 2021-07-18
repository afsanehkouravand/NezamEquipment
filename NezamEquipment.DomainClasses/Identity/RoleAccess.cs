using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Base;

namespace NezamEquipment.DomainClasses.Identity
{
    public class RoleAccess : BaseEntity
    {
        [Required, StringLength(100), Index]
        public string Area { get; set; }

        [Required, StringLength(100), Index]
        public string Controller { get; set; }

        [Required, StringLength(100), Index]
        public string Action { get; set; }

        public bool HaveAccess { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(CreateById))]
        public User CreateBy { get; set; }
        public Guid? CreateById { get; set; }

    }
}
