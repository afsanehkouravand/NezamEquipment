using System.Web.Mvc;

namespace MyCommon.Helpers.Ul
{
    public static class UlEmptyTabHelper
    {
        public static MvcHtmlString UlEmptyTab(this HtmlHelper html, string id)
        {
            var divTabPanel = new TagBuilder("div");
            divTabPanel.AddCssClass("tab-pane");
            divTabPanel.MergeAttribute("role", "tabpanel");
            divTabPanel.MergeAttribute("id", id.ToLower());

            var divLoading = new TagBuilder("div"){InnerHtml = ""};
            divLoading.AddCssClass("text-center well well-shadow loading");

            var i = new TagBuilder("i");
            i.AddCssClass("fa fa-refresh fa-spin fa-5x margin-bottom-10 display-block");

            divLoading.InnerHtml += i.ToString();

            var span = new TagBuilder("span");
            span.MergeAttribute("style", "display:block");
            span.SetInnerText("لطفا صبر کنید");

            divLoading.InnerHtml += span.ToString();

            divTabPanel.InnerHtml = divLoading.ToString();

            return new MvcHtmlString(divTabPanel.ToString());
        }
        public static MvcHtmlString UlEmptyTab(this HtmlHelper html, EUlTabId id)
        {
            var divTabPanel = new TagBuilder("div");
            divTabPanel.AddCssClass("tab-pane");
            divTabPanel.MergeAttribute("role", "tabpanel");
            divTabPanel.MergeAttribute("id", id.ToString().ToLower());

            var divLoading = new TagBuilder("div") { InnerHtml = "" };
            divLoading.AddCssClass("text-center well well-shadow loading");

            var i = new TagBuilder("i");
            i.AddCssClass("fa fa-refresh fa-spin fa-5x margin-bottom-10 display-block");

            divLoading.InnerHtml += i.ToString();

            var span = new TagBuilder("span");
            span.MergeAttribute("style", "display:block");
            span.SetInnerText("لطفا صبر کنید");

            divLoading.InnerHtml += span.ToString();

            divTabPanel.InnerHtml = divLoading.ToString();

            return new MvcHtmlString(divTabPanel.ToString());
        }
    }
}