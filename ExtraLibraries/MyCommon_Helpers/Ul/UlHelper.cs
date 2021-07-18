using System.Web.Mvc;

namespace MyCommon.Helpers.Ul
{
    public static class UlHelper
    {
        public static UlExtension Ul<TModel>(this HtmlHelper<TModel> htmlHelper, object model)
        {
            return new UlExtension(htmlHelper, model);
        }
    }
}