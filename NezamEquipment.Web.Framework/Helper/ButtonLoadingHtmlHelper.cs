using System.Web.Mvc;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class ButtonLoadingHtmlHelper
    {
        public static MvcHtmlString ButtonLoading(this HtmlHelper html, string id = "loading")
        {
            var divTag = new TagBuilder("span");
            divTag.AddCssClass("btn btn-default disabled margin-right-20 display-none");
            divTag.GenerateId(id);

            var iTag = new TagBuilder("i");
            iTag.AddCssClass("fa fa-refresh fa-spin");

            var spanTag = new TagBuilder("span");
            spanTag.SetInnerText("لطفا صبر کنید");

            divTag.InnerHtml += iTag;
            divTag.InnerHtml += spanTag;

            return new MvcHtmlString(divTag.ToString());
        }
    }
}