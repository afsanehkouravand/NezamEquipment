using System.Collections.Generic;

namespace NezamEquipment.Web.Framework.Result
{
    public class AjaxResult
    {
        public AjaxResult(bool isSucceed = true, string message = null)
        {
            IsSucceed = isSucceed;
            Message = message;
        }
        public AjaxResult(bool isSucceed = true, IList<string> message = null)
        {
            IsSucceed = isSucceed;
            ErrorMessages = message;
        }
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public IList<string> ErrorMessages { get; set; }
    }
}
