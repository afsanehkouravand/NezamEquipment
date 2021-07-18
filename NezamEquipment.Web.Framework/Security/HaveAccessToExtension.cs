using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;

namespace NezamEquipment.Web.Framework.Security
{
    public class HaveAccessToViewModel
    {
        public IList<RoleAccessDto> RoleAccess { get; set; }

        public bool IsAdmin { get; set; }

        public string CurrentArea { get; set; }

        public string CurrentController { get; set; }

    }

}
