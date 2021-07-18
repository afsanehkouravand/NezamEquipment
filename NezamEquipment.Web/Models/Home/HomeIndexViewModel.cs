using System;
using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Models.Home
{
    public class HomeIndexViewModel
    {
        public DateTime Date { get; set; }

        [Display(Name = "نام کاربری"), Required, StringLength(20)]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور"), Required, StringLength(20), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "پروفایل")]
        public LoginProfileType LoginProfileType { get; set; }
        public string Error { get; set; }
        public string Success { get; set; }

        public Guid Id { get; set; }

        public string HardSize { get; set; }
    }

    public enum LoginProfileType
    {
        /// <summary>
        /// ادمین
        /// </summary>
        [Display(Name = "انتخاب پروفایل")]
        Admin,

        /// <summary>
        /// متقاضی
        /// </summary>
        [Display(Name = "پروفایل متقاضی")]
        Applicant,

        /// <summary>
        /// فروشگاه
        /// </summary>
        [Display(Name = "پروفایل فروشگاه")]
        Store


    }
}