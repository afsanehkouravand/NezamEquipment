using NezamEquipment.ServiceLayer.OtherServices.Upload.Enum;

namespace NezamEquipment.ServiceLayer.OtherServices.Upload.Dto
{
    public class UploadByBase64Dto
    {
        public string Base64 { get; set; }

        public string Name { get; set; }

        public UploadFolderType FolderType { get; set; }

        public string Title { get; set; }
        public string CustomData { get; set; }

        public bool IsRequired { get; set; }
        public bool IsDisabled { get; set; }
    }
}
