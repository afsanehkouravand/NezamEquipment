using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Base;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.DomainClasses.Entity.Equipment;
using NezamEquipment.DomainClasses.Identity;

namespace NezamEquipment.DomainClasses.Entity.Employees
{
    public class Employe : BaseEntity
    {

        #region Properties

               
        /// <summary>
        /// نام و نام خانوادگی
        /// </summary>
        [Required, StringLength(250)]
        public string Fullname { get; set; }

        /// <summary>
        /// کد پرسنلی
        /// </summary>
       
        public int? CodePersonal { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        [Required, StringLength(20), Index]
        public string NationalCode { get; set; }

        /// <summary>
        ///  نام واحد
        /// </summary>
        public UnitType unitType { get; set; }

        /// <summary>
        /// جنسیت
        /// </summary>
        [Required]
        public GenderType GenderType { get; set; }

        /// <summary>
        /// ملیت
        /// </summary>
        public NationalityType NationalityType { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// نام پدر
        /// </summary>
        [StringLength(200)]

        public string FatherName { get; set; }

        /// <summary>
        /// محل تولد
        /// </summary>
        [StringLength(200)]
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        [StringLength(20)]
        public string CertificateCode { get; set; }

        /// <summary>
        /// شماره تماس
        /// </summary>
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        [Required, StringLength(50)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [StringLength(5000)]
        public string Description { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        [StringLength(1000)]
        public string Address { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        [StringLength(20)]
        public string PostalCode { get; set; }

        /// <summary>
        /// ایمیل
        /// </summary>
        [StringLength(200)]
        public string Email { get; set; }

        /// <summary>
        ///  اجازه لاگین
        /// </summary>
        [Index]
        public LoginStatus LoginStatus { get; set; }

        /// <summary>
        ///  Ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        ///  نوع شبکه
        /// </summary>
        public NetworkType NetworkType { get; set; }
        
        /// <summary>
        ///   نام کاربری
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// رمز عبور ورود به سیستم
        /// </summary>
        [StringLength(100)]
        public string Password { get; set; }

        /// <summary>
        /// کلید امنیتی رمز عبور
        /// </summary>
        public byte[] PasswordSalt { get; set; }

        /// <summary>
        ///user
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
        #endregion

        #region Navigation Properties

        public virtual ICollection<Equipment.Equipment> Equipment { get; set; }
        #endregion

    }
}
