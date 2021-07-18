using System.Web.Mvc;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class DefaultUlEmptyTabHelper
    {
        public static MvcHtmlString DefaultUlEmptyTab(this HtmlHelper html, string id)
        {
            var divTabPanel = new TagBuilder("div");
            divTabPanel.AddCssClass("tab-pane");
            divTabPanel.MergeAttribute("role", "tabpanel");
            divTabPanel.MergeAttribute("id", id);

            var divLoading = new TagBuilder("div"){InnerHtml = ""};
            divLoading.AddCssClass("text-center well well-shadow loading");

            var i = new TagBuilder("i");
            i.AddCssClass("fa fa-refresh fa-spin fa-5x margin-bottom-10 display-block");

            divLoading.InnerHtml += i.ToString();

            var span = new TagBuilder("span");
            span.SetInnerText("لطفا صبر کنید");

            divLoading.InnerHtml += span.ToString();

            divTabPanel.InnerHtml = divLoading.ToString();

            return new MvcHtmlString(divTabPanel.ToString());
        }
    }
}