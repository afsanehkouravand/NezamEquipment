using System.Collections.Generic;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.Role
{
    public class AdminRoleIndexViewModel : AdminBaseViewModel
    {
        public AdminRoleIndexViewModel()
        {
            Role = new RoleDto();
        }

        public RoleDto Role { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }

        public IEnumerable<RoleDto> DefaultRoles { get; set; }

    }
}