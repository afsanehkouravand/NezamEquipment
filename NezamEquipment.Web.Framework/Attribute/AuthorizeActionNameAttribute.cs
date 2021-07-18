using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;

namespace NezamEquipment.Web.Framework.Attribute
{
    public class AuthorizeActionNameAttribute : FilterAttribute
    {
        public string Name;
        public string Description;
        public string IsDuplicateTo { get; set; }

        public AuthorizeActionNameAttribute(string name = "", string description = null, string isDuplicateTo = null)
        {
            Name = name;
            Description = description;
            IsDuplicateTo = isDuplicateTo;
        }

        public AuthorizeActionNameAttribute(Title title, string description = null, int order = 0)
        {
            Name = title.GetDisplayName();
            Description = description;
            Order = order == 0 ? (int) title : order;
        }

        public enum Title
        {
            /// <summary>
            /// لیست اطلاعات
            /// </summary>
            [Display(Name = "لیست اطلاعات")]
            Index = 0,

            /// <summary>
            /// نمایش اطلاعات
            /// </summary>
            [Display(Name = "نمایش اطلاعات")]
            Details = 1,

            /// <summary>
            /// ایجاد مورد جدید
            /// </summary>
            [Display(Name = "ایجاد مورد جدید")]
            Add = 2,

            /// <summary>
            /// ویرایش اطلاعات
            /// </summary>
            [Display(Name = "ویرایش اطلاعات")]
            Edit = 3,

            /// <summary>
            /// حذف اطلاعات
            /// </summary>
            [Display(Name = "حذف اطلاعات")]
            Delete = 4,

            /// <summary>
            /// نمایش گزارش
            /// </summary>
            [Display(Name = "نمایش گزارش")]
            Report = 5,
                /// <summary>
                /// نمایش گزارش
                /// </summary>
                [Display(Name = "لیست اطلاعات باز ارسال شده ها")]
            Resend = 6
        }

    }
}
