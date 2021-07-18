using System.Collections.Generic;
using NezamEquipment.Web.Areas.Admin.Base;
using MyCommon.Helpers.Grid;
using NezamEquipment.ServiceLayer.Entity.Employees.Dto;

namespace NezamEquipment.Web.Areas.Admin.Features.Employees
{
    public class AdminNezamEmployeIndexViewModel : AdminBaseViewModel, IPagingData
    {
        public AdminNezamEmployeIndexViewModel()
        {
            NezamEmploye = new EmployeDto();
        }

        public EmployeDto NezamEmploye { get; set; }

        public IEnumerable<EmployeDto> NezamEmployes { get; set; }

        public int PageTotal { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public AdminNezamEmployeIndexSearchViewModel S { get; set; }
    }
}
