using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto
{
 
    public class TransferDto : BaseEntityDto
    {

        /// <summary>
        /// شماره قرارداد
        /// </summary>
        [Display(Name = "شماره قرارداد"), Required]
        public string ContractNumber { get; set; }

        /// <summary>
        /// تاریخ شروع اعتبار
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "تاریخ شروع اعتبار")]
        public DateTime? ContractDate { get; set; }

        /// <summary>
        /// نام بهره بردار
        /// </summary>
        [Display(Name = "نام بهره بردار")]
        public string OthersContractParty { get; set; }


        /// <summary>
        /// شماره ملی
        /// </summary>
        [Display(Name = "شماره ملی فروشنده")]
         public string NationalCode { get; set; }
        [Display(Name = "نام فروشنده")]
        public string Fullname { get; set; }
        /// <summary>
        /// نام پدر
        /// </summary>
        [StringLength(200)]
        [Display(Name = "نام پدر")]
        public string FatherName { get; set; }


        /// <summary>
        /// تاریخ انقضا مجوز
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "تاریخ انقضا مجوز")]
        public DateTime? ExpiredExpirationDate { get; set; }

        /// <summary>
        /// عنوان فعالیت
        /// </summary>
        [Display(Name = "عنوان فعالیت")]
        public string TitleOfActivity { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        [Display(Name = "آدرس")]
        public string Address { get; set; }

        /// <summary>
        /// پلاک ثبتی
        /// </summary>
        [Display(Name = "پلاک ثبتی")]
        public string RegisterPlak { get; set; }
        
        /// <summary>
        ///  بخش
        /// </summary>
        [Display(Name = "بخش")]
        public string Bakhsh { get; set; }
        /// <summary>
        ///  کدپستی
        /// </summary>
        [Display(Name = "کدپستی")]
        public string PostalCode { get; set; }

        /// <summary>
        ///  مساحت
        /// </summary>
        [Display(Name = "مساحت")]
        public string Area { get; set; }

        /// <summary>
        ///  کاربری
        /// </summary>
        [Display(Name = "کاربری")]
        public TransferUserType UserType { get; set; }

        
        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات")]
        public string Description { get; set; }


        /// <summary>
        ///وضعیت مجوز
        /// </summary>
        [Display(Name = "وضعیت مجوز")]
        public TransferStatus TransferStatus { get; set; }

        /// <summary>
        /// مراحل تایید
        /// </summary>
        [Display(Name = "مراحل تایید")]
        public TransferStatusNew TransferStatusNew { get; set; }

        /// <summary>
        /// مراحل تایید
        /// </summary>
        [Display(Name = "مراحل تایید")]
        public TransferStep VerificationStep { get; set; }

        /// <summary>
        ///وضعیت 
        /// </summary>
        [Display(Name = "وضعیت")]
        public TransferStepNew VerificationStepNew { get; set; }

        /// <summary>
        ///وضعیت کارشناسی 
        /// </summary>
        [Display(Name = "مراحل تایید")]
        public TransferStep? VerificationStepOriginal { get; set; }   
        
        /// <summary>
        ///وضعیت کارشناسی 
        /// </summary>
        [Display(Name = "مراحل تایید")]
        public TransferStepNew? VerificationStepOriginalNew { get; set; }

        /// <summary>
        /// رد توسط کارشناس
        /// </summary>
        [Display(Name = "رد توسط کارشناس")]
        public bool IsSuspension { get; set; }


        /// <summary>
        /// رد توسط کارشناس
        /// </summary>
        [Display(Name = "رد توسط کارشناس")]
        public TransferStep? SuspensionStep { get; set; }
         /// <summary>
        /// رد توسط کارشناس
        /// </summary>
        [Display(Name = "رد توسط کارشناس")]
        public TransferStepNew? SuspensionStepNew { get; set; }

        /// <summary>
        /// ارسال مجدد توسط متقاضی
        /// </summary>
        [Display(Name = "ارسال مجدد توسط متقاضی")]
        public bool IsSuspensionReSended { get; set; }


        /// <summary>
        ///ای دی متقاضی
        /// </summary>
        public Guid NezamEmployeId { get; set; }

        /// <summary>
        ///عرصه ملک
        /// </summary>
        /// 
        [Display(Name = " ارزش عرصه ")]
        public Double ArenaEarth { get; set; }
        /// <summary>
        ///اعیان ملک
        /// </summary>
        [Display(Name = " ارزش اعیان")]
        public Double LandEarth { get; set; }
        /// <summary>
        ///مساحت ملک
        /// </summary>
        [Display(Name = "مساحت عرصه")]
        public Double LandArenaArea { get; set; }

        /// <summary>
        ///مساحت ملک
        /// </summary>
        [Display(Name = "مساحت اعیان")]
        public Double LandArea { get; set; }

        /// <summary>
        ///مجموعه عرصه و اعیان ملک
        /// </summary>
        [Display(Name = "محموعه عرصه و اعیان ملک")]
        public Double ArenaAndLand { get; set; }
        /// <summary>
        ///5 درصد عرصه و اعیان ملک
        /// </summary>
        [Display(Name = "5 % عرصه و اعیان ")]
        public Double? FivePercent { get; set; }
        /// <summary>
        ///2 درصد عرصه و اعیان ملک
        /// </summary>
        [Display(Name = "2 % عرصه و اعیان ")]
        public Double? TowPercent { get; set; }
        /// <summary>
        ///5 درصد عرصه و اعیان ملک
        /// </summary>
        [Display(Name = "5 % عرصه و اعیان ")]
        public Double FivePercentArenaAndLand { get; set; }
        /// <summary>
        ///2 درصد عرصه و اعیان ملک
        /// </summary>
        [Display(Name = "2 % عرصه و اعیان ")]
        public Double TowPercentArenaAndLand { get; set; }
        /// <summary>
        ///عوارض نقل و انتقال
        /// </summary>
        [Display(Name = "عوارض نقل و انتقال")]
        public Double DuesTransfer { get; set; }
        /// <summary>
        ///ارزش کل
        /// </summary>
        [Display(Name = "ارزش کل")]
        public Double TotalValue { get; set; }
        /// <summary>
        ///ارزش تجاری
        /// </summary>
        [Display(Name = "ارزش تجاری")]
        public Double CommercialValue { get; set; }
        /// <summary>
        ///ارزش تجاری
        /// </summary>
        [Display(Name = "میزان مالکیت")]
        public Ownership  Ownership { get; set; }
        public bool IsShow { get; set; }
        public bool Resend { get; set; }
        [Display(Name = " میزان مالکیت خاص")]
        public double SpecialOwnership { get; set; }
        /// <summary>
        /// تاریخ تایید مالی
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "تاریخ تایید مالی")]
        public DateTime? FinancialDate { get; set; }

        [Display(Name = "تاریخ تایید مالی")]
        public string FinancialDateStr1 { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات")]
        public string DescriptionFinancial1 { get; set; }

        [Display(Name = "تایید مالی")]
        public bool? IsFinancial1 { get; set; }

        [Display(Name = "آیا فروشنده بیش از یک نفر است ")]
        public NumberSeller NumberSeller { get; set; }


    }
}
