using System.Web.Mvc;

namespace MyCommon.Helpers.ToolBox
{
    public static class ToolBoxHelper
    {
        public static ToolBoxExtension ToolBox<TModel>(this HtmlHelper<TModel> htmlHelper, object model)
        {
            return new ToolBoxExtension(htmlHelper: htmlHelper, model: model);
        }
    }
}