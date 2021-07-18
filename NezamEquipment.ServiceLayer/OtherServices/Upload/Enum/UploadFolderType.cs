using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.ServiceLayer.OtherServices.Upload.Enum
{
    public enum UploadFolderType
    {
        [Display(Name = "هیچ")]
        None = 0,
        
        [Display(Name = "فاکتور ")]
        FactorDoc
    

    }
}