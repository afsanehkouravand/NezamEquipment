using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml
{
    public class SettingMailXml
    {
        public SettingMailXml()
        {
            ClientHost = "";
            From = "";
            Title = "";
            UserName = "";
            Password = "";
            UserName = "";
            Port = 0;
            EnableSsl = false;
        }

        [Display(Name = "هاست")]
        [AdditionalMetadata("Help", "آدرس سروری که قرار است ایمیل ها با آن ارسال شوند را وارد کنید.")]
        public string ClientHost { get; set; }

        [Display(Name = "نام کاربری")]
        [AdditionalMetadata("Help", "نام کاربر مجاز برای ورود به سرور را وارد کنید.")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        [AdditionalMetadata("Help", "رمز عبور سرور را وارد کنید.")]
        public string Password { get; set; }

        [Display(Name = "پورت")]
        [AdditionalMetadata("Help", "پورت سرور را وارد کنید.")]
        public int Port { get; set; }

        [Display(Name = "پروتکل SSL")]
        [AdditionalMetadata("Help", "اگر از پروتکل SSL استفاده میکنید، این گزینه را انتخاب کنید.")]
        public bool EnableSsl { get; set; }

        [Display(Name = "از طرفه")]
        [AdditionalMetadata("Help", "ایمیلی که میخواهید به عنوان فرستنده نمایش داده شود را وارد کنید.")]
        public string From { get; set; }

        [Display(Name = "عنوان")]
        [AdditionalMetadata("Help", "عنوان ایمیل های ارسالی را وارد کنید.")]
        public string Title { get; set; }

    }
}