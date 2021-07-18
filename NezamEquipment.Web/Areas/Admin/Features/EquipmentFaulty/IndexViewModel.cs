using System.Collections.Generic;
using NezamEquipment.Web.Areas.Admin.Base;
using MyCommon.Helpers.Grid;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile.Dto;

namespace NezamEquipment.Web.Areas.Admin.Features.EquipmentFaulty
{
    public class AdminEquipmentFaultyIndexViewModel : AdminBaseViewModel, IPagingData
    {
        public AdminEquipmentFaultyIndexViewModel()
        {
            EquipmentFaultyDto = new EquipmentFaultyDto();
            EquipmentFaultyFile = new EquipmentFaultyFileDto();
        }

        public EquipmentFaultyDto EquipmentFaultyDto { get; set; }

        public IEnumerable<EquipmentFaultyDto> EquipmentFaultyDtos { get; set; }

        public string ImagePath { get; set; }
        public EquipmentFaultyFileDto EquipmentFaultyFile { get; set; }
        public IEnumerable<EquipmentFaultyFileDto> EquipmentFaultyFiles { get; set; }
        public int PageTotal { get; set; }
        public int PageSize { get; set; }
      
      public int PageNumber { get; set; }
      
       
        public AdminEquipmentFaultyIndexSearchViewModel S { get; set; }
    }
}
