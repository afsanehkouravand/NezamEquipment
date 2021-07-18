using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Areas.Admin.Base
{
    public class AdminBaseViewModel
    {
        public HaveAccessToViewModel HaveAccessTo { get; set; }

        public UserDto CurrentUser { get; set; }

        public string LayoutTitle { get; set; }

    }
}