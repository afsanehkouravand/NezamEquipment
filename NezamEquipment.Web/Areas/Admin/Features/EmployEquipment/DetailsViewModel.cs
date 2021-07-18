using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.EmployEquipment
{
    public class AdminNezamEquipmentDetailsViewModel : AdminBaseViewModel
    {
        public EquipmentDto NezamEquipment { get; set; }
    }
}
