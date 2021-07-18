using System.ComponentModel.DataAnnotations;
using NezamEquipment.Common.Extension;

namespace NezamEquipment.ServiceLayer
{
    public class DbResult
    {
        public DbResult()
        {
            Status = true;
        }

        public DbResult(bool status)
        {
            Status = status;
        }

        public DbResult(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        public DbResult(string message)
        {
            Status = false;
            Message = message;
        }

        public DbResult(M message)
        {
            Status = false;
            Message = message.GetDisplayName();
        }

        public bool Status { get; set; }
        public string Message { get; set; }

        public static DbResult GetFalse(string message = "")
        {
            return new DbResult(false, message);
        }

        public enum M
        {
            /// <summary>
            /// اطلاعات مورد نظر یافت نشد.اطلاعات مورد نظر یافت نشد.
            /// </summary>
            [Display(Name = "اطلاعات مورد نظر یافت نشد.")]
            NotFound ,

            /// <summary>
            /// امکان ذخیره اطلاعات وجود ندارد.
            /// </summary>
            [Display(Name = "امکان ذخیره اطلاعات وجود ندارد.")]
            CanNotSave,

            /// <summary>
            /// اطلاعات مورد نظر از قبل در پایگاه داده دخیره شده است.
            /// </summary>
            [Display(Name = "اطلاعات مورد نظر از قبل در پایگاه داده دخیره شده است.")]
            AlreadyExist,

            /// <summary>
            /// اطلاعات ارسال شده صحیح نمی باشند.
            /// </summary>
            [Display(Name = "اطلاعات ارسال شده صحیح نمی باشند.")]
            InCorrectAccess,
        }
    }
}
