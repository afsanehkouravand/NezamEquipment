using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.Home
{
    public class AdminNezamHomeDetailsViewModel : AdminBaseViewModel
    {
        public EquipmentDto NezamEquipment { get; set; }
    }
}
