using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.User
{
    public class AdminUserDetailsViewModel : AdminBaseViewModel
    {
        public UserDto User { get; set; }

        [Display(Name = "نقش ها")]
        public IEnumerable<RoleDto> Roles { get; set; }
    }
}
