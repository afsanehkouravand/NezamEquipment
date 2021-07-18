using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.Common.Extension;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;

namespace NezamEquipment.Web.Areas.Admin.Features.EquipmentFaulty
{
    public class AdminEquipmentFaultyPartialFormViewModel
    {
        public AdminEquipmentFaultyPartialFormViewModel()
        {
            UploadByBase64 = new UploadByBase64Dto()
            { 
                Title = EquipmentFaultyFileType.FactorDocument.GetDisplayName(),
                    CustomData = ((int)EquipmentFaultyFileType.FactorDocument).ToString(),
                    IsRequired = true,
               
            };

            }
        public UploadByBase64Dto UploadByBase64 { get; set; }
        public EquipmentFaultyDto EquipmentFaultyDto { get; set; }
       
        public Guid EquipmentId { get; set; }

        [Display(Name = "انتخاب کارمند")]
        public IDictionary<string, string> DropDown { get; set; }


    }
}
