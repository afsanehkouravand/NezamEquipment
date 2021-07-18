using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto
{
 
    public class TransferIssuingPermitDto : BaseEntityDto
    {


        /// <summary>
        /// تاریخ صدور مجوز
        /// </summary>
       
   
        [Display(Name = "تاریخ صدور "), Required]
        [DataType(DataType.Date)]
        public DateTime IsDueDate { get; set; }

        [Display(Name = "تاریخ صدور "), Required]
        [DataType(DataType.Date)]
        public string IsDueDateStr { get; set; }

        [Display(Name = "شماره مجوز"), Required, StringLength(20)]
        public string LicenseNumber { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات ")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        ///نقل و انقال
        /// </summary>
        [ForeignKey(nameof(TransferId))]
        public virtual DomainClasses.Entity.Transfer.Transfer Transfer { get; set; }
      
        public Guid TransferId { get; set; }

        /// <summary>
        /// کاربر سازنده
        /// </summary>
        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User CreatedByUser { get; set; }
        public Guid? CreatedByUserId { get; set; }

        /// <summary>
        /// نام بهره بردار
        /// </summary>
        [Display(Name = "نام بهره بردار ")]
        public string FullName { get; set; }

        /// <summary>
        /// کد ملی بهره بردار
        /// </summary>
        [Display(Name = "کد ملی بهره بردار ")]
        public string NationalCode { get; set; }
        /// <summary>
        /// شماره قرارداد
        /// </summary>
        [Display(Name = "شماره قراداد ")]
        public string ContractNumber { get; set; }
        /// <summary>
        /// نام بهره بردار
        /// </summary>
        [Display(Name = "نام پدر ")]
        public string FatherName { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        [Display(Name = " آدرس")]
        public string Address { get; set; }
        /// <summary>
        /// QrCode
        /// </summary>
        [Display(Name = " QrCode")]
        public string QrCode { get; set; }
    }
}
