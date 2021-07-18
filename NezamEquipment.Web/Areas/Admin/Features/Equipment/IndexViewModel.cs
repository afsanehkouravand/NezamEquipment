using System.Collections.Generic;
using NezamEquipment.Web.Areas.Admin.Base;
using MyCommon.Helpers.Grid;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.DomainClasses.Enum;

namespace NezamEquipment.Web.Areas.Admin.Features.Equipment
{
    public class AdminNezamEquipmentIndexViewModel : AdminBaseViewModel, IPagingData
    {
        public AdminNezamEquipmentIndexViewModel()
        {
            NezamEquipment = new EquipmentDto();
        }

        public EquipmentDto NezamEquipment { get; set; }

        public IEnumerable<EquipmentDto> NezamEquipments { get; set; }
        public UnitType unitType { get; set; }

        public int PageTotal { get; set; }
        public int PageSize { get; set; }
        public int CaseTotal { get; set; }
        public int MonitorTotal { get; set; }
        public int PrinterTotal { get; set; }
        public int ScanerTotal { get; set; }
        public int LaptopTotal { get; set; }
        public int AllinoneTotal { get; set; }
        public int PageNumber { get; set; }
        public int TabletTotal { get; set; }
        public int mouseTotal { get; set; }
        public int KeyTotal { get; set; }
        public int HardTotal { get; set; }
        public int FaxTotal { get; set; }
       
        public AdminNezamEquipmentIndexSearchViewModel S { get; set; }
    }
}
