using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml
{
    public class SettingOverAllXml
    {

        public SettingOverAllXml()
        {
            Title = "";
            StoreBuyerCitizen = 0;
            StoreBuyerNotCitizen = 0;
        }

        [Display(Name = "نام")]
        [AdditionalMetadata("Help", "لطفا نام وب سایت را در اینجا وارد کنید.")]
        public string Title { get; set; }

        [Display(Name = "سیف مجاز شهروندان")]
        [AdditionalMetadata("Help", "میزان سیف مجاز برای خرید شهروندان منطقه")]
        public int StoreBuyerCitizen { get; set; }

        [Display(Name = "سیف مجاز غیر شهروندان")]
        [AdditionalMetadata("Help", "میزان سیف مجاز برای خرید افراد غیر شهروند در منطقه")]
        public int StoreBuyerNotCitizen { get; set; }
    }
}
