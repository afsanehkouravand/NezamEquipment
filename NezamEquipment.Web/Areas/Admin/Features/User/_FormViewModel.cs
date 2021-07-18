using System.Collections.Generic;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer._Identity.User.Dto;

namespace NezamEquipment.Web.Areas.Admin.Features.User
{
    public class AdminUserPartialFormViewModel
    {
        public UserDto User { get; set; }

        public IEnumerable<SelectListItem> AllRoles { get; set; }

        public IList<string> SelectedRole { get; set; }

    }
}
