using System.Collections.Generic;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using MyCommon.Helpers.Grid;

namespace NezamEquipment.Web.Areas.Admin.Features.User
{
    public class AdminUserIndexViewModel : AdminBaseViewModel, IPagingData
    {
        public AdminUserIndexViewModel()
        {
            User = new UserDto();
        }

        public RoleDto Role { get; set; }

        public UserDto User { get; set; }

        public IEnumerable<UserDto> Users { get; set; }

        public int PageTotal { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public AdminUserIndexSearchViewModel S { get; set; }
    }
}
