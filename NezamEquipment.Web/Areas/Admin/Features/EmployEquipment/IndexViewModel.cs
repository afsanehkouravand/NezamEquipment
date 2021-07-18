using System.Collections.Generic;
using NezamEquipment.Web.Areas.Admin.Base;
using MyCommon.Helpers.Grid;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;

namespace NezamEquipment.Web.Areas.Admin.Features.EmployEquipment
{
    public class AdminEmployEquipmentIndexViewModel : AdminBaseViewModel
    {
        public AdminEmployEquipmentIndexViewModel()
        {
            NezamEquipment = new EquipmentDto();
        }
        public IEnumerable<EquipmentDto> NezamEquipments { get; set; }
        public EquipmentDto NezamEquipment { get; set; }

          }
}
