using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.Profile
{
    public class AdminProfileEditViewModel : AdminBaseViewModel
    {
        public UserDto User { get; set; }
    }
}
