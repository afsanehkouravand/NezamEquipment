using System.Collections.Generic;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Areas.Admin.Features.Role
{
    public class AdminRolePartialFormViewModel : AdminBaseViewModel
    {
        public RoleDto Role { get; set; }

        public IEnumerable<GetAllAreaControllerAction.AreaAuth> AllAreaWithController { get; set; }

        public IEnumerable<RoleAccessDto> ListOfAccesses { get; set; }

        public RoleAccessDto RoleAccess { get; set; }
    }
}