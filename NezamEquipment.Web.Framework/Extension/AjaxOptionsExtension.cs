using System.Web.Mvc.Ajax;

namespace NezamEquipment.Web.Framework.Extension
{
    public static class AjaxOptionsExtension
    {
        public static AjaxOptions Get(string url, string loadingElementId = "loading",
            string onSuccess = "OnSuccess", string onComplete = "OnComplete",string onFailure = "OnFailure", 
            string onBegin = "OnBegin", string httpMethod = "post", string updateTargetId = null)
        {
            return new AjaxOptions()
            {
                Url = url,
                HttpMethod = httpMethod,
                OnBegin = onBegin,
                OnFailure = onFailure,
                OnSuccess = onSuccess,
                OnComplete = onComplete,
                LoadingElementId = loadingElementId,
                UpdateTargetId = updateTargetId,
            };
        }
    }
}