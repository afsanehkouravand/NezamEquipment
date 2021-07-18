using System.Collections.Generic;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Areas.Admin.Features.UserAccessList
{
    public class AdminUserAccessListIndexViewModel : AdminBaseViewModel
    {
        public AdminUserAccessListIndexViewModel()
        {
            RoleAccess = new RoleAccessDto();
            User = new UserDto();
        }

        public RoleAccessDto RoleAccess { get; set; }
        public IEnumerable<RoleAccessDto> RoleAccesses { get; set; }
        public UserDto User { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
        public IEnumerable<GetAllAreaControllerAction.AreaAuth> AllAreaWithController { get; set; }

        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
