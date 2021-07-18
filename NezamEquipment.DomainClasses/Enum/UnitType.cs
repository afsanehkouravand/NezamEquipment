using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum UnitType
    {
       
        /// <summary>
              /// نظام مهندسی
              /// </summary>
        [Display(Name = "نظام مهندسی")]
        None,
        /// <summary>
        /// ریاست
        /// </summary>
        [Display(Name = "دفتر ریاست")]
        boss,
  
        /// <summary>
        /// خدمات مهندسی
        /// </summary>
        [Display(Name = "خدمات مهندسی")]
        KhadamatMohandesi,
        /// <summary>
        /// شورای انتظامی
        /// </summary>
        [Display(Name = "شورای انتظامی")]
        ShoraiEntezami,
        /// <summary>
        /// اداری
        /// </summary>
        [Display(Name = "اداری")]
        adari,
        /// <summary>
        /// عضویت
        /// </summary>
        [Display(Name = "عضویت و پروانه اشتغال")]
        Ozviat,
        /// <summary>
        /// آموزش
        /// </summary>
        [Display(Name = "آموزش")]
        Amozesh,
        /// <summary>
        /// انفورماتیک
        /// </summary>
        [Display(Name = "انفورماتیک")]
        It,
        /// <summary>
        /// مالی
        /// </summary>
        [Display(Name = "مالی")]
        Mali,
        /// <summary>
        /// دبیرخانه
        /// </summary>
        [Display(Name = "دبیرخانه")]
        DabirKhane,
        /// <summary>
        /// برق
        /// </summary>
        [Display(Name = "برق")]
        Bargh,
        /// <summary>
        /// گاز
        /// </summary>
        [Display(Name = "گاز")]
        Gas,
        /// <summary>
        /// ژئوتکنیک
        /// </summary>
        [Display(Name = "ژئوتکنیک")]
        Zheo,
        /// <summary>
        /// روابط عمومی
        /// </summary>
        [Display(Name = "روابط عمومی")]
        RavabetOmomi,
        /// <summary>
        ///  مدیریت واحد تاسیسات
        /// </summary>
        [Display(Name = " مدیریت واحد تاسیسات")]
        Tasisat,
        /// <summary>
        ///   کمیته
        /// </summary>
        [Display(Name = " کمیته ")]
        Komite, 
        /// <summary>
         ///   کمیته نظارت و کنترل
         /// </summary>
        [Display(Name = " کمیته نظارت و کنترل")]
       Nezarat,
        /// <summary>
        ///  رفاه 
        /// </summary>
        [Display(Name = "رفاه")]
        Refah,
        /// <summary>
        ///  امور مجریان ذی صلاح 
        /// </summary>
        [Display(Name = "امور مجریان ذی صلاح")]
        Mojrian,

        /// <summary>
        ///   ماده 27
        /// </summary>
        [Display(Name = "ماده 27")]
        Made27,
        /// <summary>
        ///    ساختمان موحدین
        /// </summary>
        [Display(Name = "  ساختمان موحدین")]
        SakhtemanMovahedi,

    }
}