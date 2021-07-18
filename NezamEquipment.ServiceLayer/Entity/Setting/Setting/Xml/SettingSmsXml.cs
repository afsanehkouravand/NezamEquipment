using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NezamEquipment.Common.Attribute;

namespace NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml
{
    public class SettingSmsXml
    {
        public SettingSmsXml()
        {
            Signature = "سازمان نظام مهندسی ساختمان خوزستان";
            ApplicantRegister = string.Empty;
            SmsForChangePassword = "";
            //Password = "";
        }

        [Display(Name = "امضا پیامک ها")]
        [AdditionalMetadata("Help", "متنی که در انتهای همه پیامک ها ارسال خواهد شد.")]
        public string Signature { get; set; }



        [DataType(DataType.MultilineText)]
        [Display(Name = "ثبت نام متقاضی")]
        [AdditionalMetadata("Help", "متن پیامکی که در آن رمز عبور متقاضی ارسال می شود. پشتیبانی از: [name] [code]")]
        public string ApplicantRegister { get; set; }

        //[Display(Name = "رمز عبور")]
        //[AdditionalMetadata("Help", "رمز عبور سامانه پیامک ها")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        [Display(Name = "پیامک تغییر رمز عبور")]
        [DataType(DataType.MultilineText)]
        [ConvertToBase64String]
        public string SmsForChangePassword { get; set; }

        [Display(Name = "پیامک تایید مبلغ عوارض نقل و انتقال")]
        [DataType(DataType.MultilineText)]
        [ConvertToBase64String]
        public string SmsDuesTransfer { get; set; }
        [Display(Name = "پیامک بازگشت به متقاضی")]
        [DataType(DataType.MultilineText)]
        [ConvertToBase64String]
        public string SmsRdirectTransfer { get; set; }
        [Display(Name = "پیامک تایید  نهایی نقل و انتقال")]
        [DataType(DataType.MultilineText)]
        [ConvertToBase64String]
        public string SmsConfirmedTransfer { get; set; }
      
        [DataType(DataType.MultilineText)]
        [Display(Name = "تایید موبایل  برای ثبت نام شرکت")]
        [AdditionalMetadata("Help", "متن تایید موبایل  برای ثبت نام شرکت، پشتیبانی از: [code],[name],[trackingcode]")]
        public string RenterRegister { get; set; }
    }
}
