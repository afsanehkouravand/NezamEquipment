using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.Employees.Dto
{
   
    public class EmployeDto : BaseEntityDto
    {


        /// <summary>
        /// نام و نام خانوادگی
        /// </summary>
        [Required, StringLength(250)]
        [Display(Name = "نام و نام خانوادگی")]
        public string Fullname { get; set; }
      
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        [Display(Name = "کد پرسنلی")]
        public int? CodePersonal { get; set; }


        /// <summary>
        /// کد ملی
        /// </summary>
        [Required, StringLength(20)]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        /// <summary>
        ///  نام واحد
        /// </summary>
        [Display(Name = "نام واحد")]
        public UnitType unitType { get; set; }

        /// <summary>
        /// جنسیت
        /// </summary>
        [Required]
        [Display(Name = "جنسیت")]
        public GenderType GenderType { get; set; }

        /// <summary>
        /// ملیت
        /// </summary>
        [Display(Name = "ملیت")]
        public NationalityType NationalityType { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// نام پدر
        /// </summary>
        [StringLength(200)]
        [Display(Name = "نام پدر")]
        public string FatherName { get; set; }

        /// <summary>
        /// محل تولد
        /// </summary>
        [StringLength(200)]
        [Display(Name = " محل تولد")]
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        [StringLength(20)]
        [Display(Name = "شماره شناسنامه")]
        public string CertificateCode { get; set; }

        /// <summary>
        /// شماره تماس
        /// </summary>
        [StringLength(50)]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        [Required, StringLength(50)]
        [Display(Name = "شماره موبایل")]
        public string MobileNumber { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [StringLength(5000)]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "آدرس")]
        public string Address { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        [StringLength(20)]
        [Display(Name = "کد پستی")]
        public string PostalCode { get; set; }

        /// <summary>
        /// ایمیل
        /// </summary>
        [StringLength(200)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        /// <summary>
        ///  اجازه لاگین
        /// </summary>
        [Index]
        [Display(Name = " اجازه لاگین")]
        public LoginStatus LoginStatus { get; set; }

        /// <summary>
        ///  Ip
        /// </summary>
        ///  
       [Display(Name = "Ip")]
        public string IP { get; set; }

        /// <summary>
        ///  نوع شبکه
        /// </summary>
        [Display(Name = "نوع شبکه")]
        public NetworkType NetworkType { get; set; }

        /// <summary>
        ///   نام کاربری
        /// </summary>
        [Display(Name = "نام کاربری")]
        public string UserLogin { get; set; }

        /// <summary>
        /// رمز عبور ورود به سیستم
        /// </summary>
        [StringLength(100)]
        [Display(Name = "رمز عبور ورود به سیستم")]
        public string Password { get; set; }

        /// <summary>
        /// کلید امنیتی رمز عبور
        /// </summary>
        public byte[] PasswordSalt { get; set; }


        [Display(Name = "تایید رمز عبور")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public Guid UserId { get; set; }
        public string UserFullName { get; set; }



    }
}
