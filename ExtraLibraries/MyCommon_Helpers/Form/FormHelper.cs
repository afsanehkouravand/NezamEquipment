using System.Web.Mvc;

namespace MyCommon.Helpers.Form
{
    public static class FormHelper
    {
        public static FormExtension Form<TModel>(this HtmlHelper<TModel> htmlHelper, object model)
        {
            return new FormExtension(htmlHelper: htmlHelper, model: model);
        }
    }
}