using NezamEquipment.Web.Areas.Admin.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.Web.Areas.Admin.Features.Equipment;

namespace NezamEquipment.Web.Areas.Admin.Features.EquipmentFaulty
{
    public class AdminEquipmentFaultyAddViewModel : AdminBaseViewModel
    {

        public AdminEquipmentFaultyAddViewModel()
        {
            PartialForm = new AdminEquipmentFaultyPartialFormViewModel();
        }
        public AdminEquipmentFaultyPartialFormViewModel PartialForm { get; set; }
       

    }
}
