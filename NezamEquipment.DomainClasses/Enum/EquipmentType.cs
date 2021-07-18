using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Enum
{
    public enum EquipmentType
    {
        /// <summary>
        /// کیس
        /// </summary>
        [Display(Name = "کیس")]
        Case = 0,

        /// <summary>
        /// مانیتور
        /// </summary>
        [Display(Name = "مانیتور")]
        Monitor,
      
        /// <summary>
        /// AllinOne
        /// </summary>
        [Display(Name = "AllinOne")]
        AllinOne,
        /// <summary>
        /// پرینتر
        /// </summary>
        [Display(Name = "پرینتر")]
        Printer,

        /// <summary>
        /// اسکنر
        /// </summary>
        [Display(Name = "اسکنر")]
        Scaner,
        
        /// <summary>
        /// لب تاب
        /// </summary>
        [Display(Name = "لب تاب")]
        Laptop,

        /// <summary>
        /// تبلت
        /// </summary>
        [Display(Name = "تبلت")]
        Tablet,

        /// <summary>
        /// هارد اکسترنال
        /// </summary>
        [Display(Name = "هارد اکسترنال")]
        HardExternal, 
        
        /// <summary>
       /// فکس       
        /// </summary>
        [Display(Name = "فکس")]
        fax,
       
        /// <summary>
        /// سوئیچ
        /// </summary>
        [Display(Name = "سوئیچ")]
        Switch,

        /// <summary>
        /// سوئیچ
        /// </summary>
        [Display(Name = "موس بیسیم")]
        MousedWireLess,
        /// <summary>
        /// سوئیچ
        /// </summary>
        [Display(Name = "کیبورد بیسیم")]
        KeyboardWireLess,
        /// <summary>
        /// مودم
        /// </summary>
        [Display(Name = "مودم")]
        Modem,
        /// <summary>
        /// اسپیکر
        /// </summary>
        [Display(Name = "اسپیکر")]
        Speaker,
        /// <summary>
        /// سیستم صوتی
        /// </summary>
        [Display(Name = "سیستم صوتی")]
        SystemSoti,
       
        /// <summary>
        ///  دستگاه کپی
        /// </summary>
        [Display(Name = "دستگاه کپی")]
        Copy,
        /// <summary>
        ///  پروژکتور
        /// </summary>
        [Display(Name = "پروژکتور")]
        Projector,
        /// <summary>
        ///  اکسس پوینت
        /// </summary>
        [Display(Name = "اکسس پوینت")]
        AccessPoint,
        /// <summary>
        ///  کیوسک
        /// </summary>
        [Display(Name = "کیوسک")]
        Kuosk,
        /// <summary>
        ///  فلش
        /// </summary>
        [Display(Name = "فلش")]
        Flash,
    }

}