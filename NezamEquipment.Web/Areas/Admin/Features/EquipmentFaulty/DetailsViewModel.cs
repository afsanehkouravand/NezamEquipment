
using System.Collections.Generic;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.EquipmentFaulty
{
    public class AdminEquipmentFaultyDetailsViewModel : AdminBaseViewModel
    {
        public EquipmentFaultyDto EquipmentFaultyDto { get; set; }
      
    }
}
