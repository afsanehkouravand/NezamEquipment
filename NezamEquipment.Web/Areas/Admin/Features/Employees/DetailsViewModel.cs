using NezamEquipment.ServiceLayer.Entity.Employees.Dto;
using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.Employees
{
    public class AdminNezamEmployeDetailsViewModel : AdminBaseViewModel
    {
        public EmployeDto NezamEmploye { get; set; }
    }
}
