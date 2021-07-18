using System.Collections.Generic;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Areas.Admin.Features.Profile
{
    public class AdminProfileIndexViewModel : AdminBaseViewModel
    {
        public UserDto User { get; set; }

        [Display(Name = "نقش ها")]
        public IEnumerable<RoleDto> Roles { get; set; }
    }
}
