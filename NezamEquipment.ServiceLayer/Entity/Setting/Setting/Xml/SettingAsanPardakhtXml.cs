using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml
{
    public class SettingAsanPardakhtXml
    {
        public SettingAsanPardakhtXml()
        {
            MerchantId = 0;
            ConfigId = 0;
            Username = "";
            Password = "";
            EncryptionKey = "";
            EncryptionVector = "";
        }

        [Display(Name = "MerchantId")]
        [AdditionalMetadata("Help", "")]
        public int MerchantId { get; set; }

        [Display(Name = "ConfigId")]
        [AdditionalMetadata("Help", "")]
        public int ConfigId { get; set; }

        [Display(Name = "Username")]
        [AdditionalMetadata("Help", "")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [AdditionalMetadata("Help", "")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "EncryptionKey")]
        [AdditionalMetadata("Help", "")]
        public string EncryptionKey { get; set; }

        [Display(Name = "EncryptionVector")]
        [AdditionalMetadata("Help", "")]
        public string EncryptionVector { get; set; }

    }
}
