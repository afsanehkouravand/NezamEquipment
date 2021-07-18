using System.Web.Mvc;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class PageTitleHelper
    {
        public static MvcHtmlString PageTitle(this HtmlHelper html, string title = "", string smallTitle = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                title = html.ViewBag.PageTitle;
            }

            var divPageHeader = new TagBuilder("div");
            divPageHeader.AddCssClass("page-header title");

            var h3 = new TagBuilder("h3") {InnerHtml = ""};
            h3.InnerHtml += title;

            if (!string.IsNullOrWhiteSpace(smallTitle))
            {
                var small = new TagBuilder("small") { InnerHtml = "" };
                small.SetInnerText(smallTitle);

                h3.InnerHtml += " " + small;
            }

            divPageHeader.InnerHtml = h3.ToString();

            return new MvcHtmlString(divPageHeader.ToString());
        }
    }
}