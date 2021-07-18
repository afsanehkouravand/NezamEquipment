using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.Home
{
    public class AdminHomeIndexViewModel : AdminBaseViewModel
    {
        //public RoleAccessDto RoleAccessDto { get; set; }

        public int EmployeCount { get; set; }
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
    }
}
