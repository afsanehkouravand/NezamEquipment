using NezamEquipment.Web.Areas.Admin.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Areas.Admin.Features.Equipment
{
    public class AdminNezamEquipmentAddViewModel : AdminBaseViewModel
    {

        public AdminNezamEquipmentAddViewModel()
        {
            PartialForm = new AdminNezamEquipmentPartialFormViewModel();
        }

        public AdminNezamEquipmentPartialFormViewModel PartialForm { get; set; }
       

    }
}
