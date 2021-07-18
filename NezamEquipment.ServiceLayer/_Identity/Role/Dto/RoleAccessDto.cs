using System;
using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.ServiceLayer._Identity.Role.Dto
{
    public class RoleAccessDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string RoleName { get; set; }
        public Guid RoleId { get; set; }

        [Required, StringLength(100)]
        public string Area { get; set; }

        [Required, StringLength(100)]
        public string Controller { get; set; }

        [Required, StringLength(100)]
        public string Action { get; set; }

        public bool? HaveAccess { get; set; }

        public string CreateByUserName { get; set; }
        public Guid? CreateById { get; set; }
    }
}
