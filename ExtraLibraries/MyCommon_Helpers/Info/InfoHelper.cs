using System.Web.Mvc;

namespace MyCommon.Helpers.Info
{
    public static class InfoHelper
    {
        public static InfoExtension Info<TModel> (this HtmlHelper<TModel> htmlHelper, object model)
        {
            return new InfoExtension(htmlHelper: htmlHelper, model: model);
        }
    }
}