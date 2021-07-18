using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Enum;

namespace NezamEquipment.ServiceLayer.OtherServices.Upload
{
    public interface IUploadService
    {
        string UploadByBase64(UploadByBase64Dto dto);
        string UploadImageByBase64(UploadByBase64Dto dto);
        bool DeleteUploadFile(string name, UploadFolderType folerType);
        string UploadImageAndPdfByBase64(UploadByBase64Dto dto);
    }
}