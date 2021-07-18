using NezamEquipment.Web.Areas.Admin.Base;

namespace NezamEquipment.Web.Areas.Admin.Features.Employees
{
    public class AdminNezamEmployeAddViewModel : AdminBaseViewModel
    {

        public AdminNezamEmployeAddViewModel()
        {
            PartialForm = new AdminNezamEmployePartialFormViewModel();
        }

        public AdminNezamEmployePartialFormViewModel PartialForm { get; set; }

    }
}
