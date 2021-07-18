using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyCommon.Helpers.Extension
{
    internal static class GetUrlInfoExtension
    {
        internal static GetUrlInfo UrlInfo(this ActionResult data, RequestContext requestContext)
        {
            return Get(data, requestContext);
        }

        internal static GetUrlInfo UrlInfo(this Task<ActionResult> data, RequestContext requestContext)
        {
            return Get(data.Result, requestContext);
        }

        internal static GetUrlInfo UrlInfo(this JsonResult data, RequestContext requestContext)
        {
            return Get(data, requestContext);
        }

        internal static GetUrlInfo UrlInfo(this Task<JsonResult> data, RequestContext requestContext)
        {
            return Get(data.Result, requestContext);
        }


        private static GetUrlInfo Get(object data, RequestContext requestContext)
        {
            var info = new GetUrlInfo();

            var dataType = data.GetType();

            //var propertyInfoAction = dataType.GetProperty("Action");
            //if (propertyInfoAction != null)
            //{
            //    var valueAction = propertyInfoAction.GetValue(data, null);
            //    if (valueAction != null)
            //    {
            //        info.Action = valueAction.ToString();
            //    }
            //}

            //var propertyInfoController = dataType.GetProperty("Controller");
            //if (propertyInfoController != null)
            //{
            //    var valueController = propertyInfoController.GetValue(data, null);
            //    if (valueController != null)
            //    {
            //        info.Controller = valueController.ToString();
            //    }
            //}

            var propertyInfoRouteValueDictionary = dataType.GetProperty("RouteValueDictionary");
            if (propertyInfoRouteValueDictionary != null)
            {
                var valueRouteValueDictionary = propertyInfoRouteValueDictionary.GetValue(data, null);
                if (valueRouteValueDictionary != null)
                {
                    var dataDictionary = (RouteValueDictionary)valueRouteValueDictionary;

                    foreach (var item in dataDictionary)
                    {
                        if (item.Key == "Area")
                        {
                            info.Area = item.Value?.ToString() ?? "";
                        }
                        else if (item.Key == "Controller")
                        {
                            info.Controller = item.Value?.ToString() ?? "";
                        }
                        else if (item.Key == "Action")
                        {
                            info.Action = item.Value?.ToString() ?? "";
                        }
                        else
                        {
                            info.QueryStrings.Add(item.Key, item.Value?.ToString() ?? "");
                        }
                    }
                }
            }

            var url = new UrlHelper(requestContext);
            info.Url = url.Action((ActionResult)data);

            return info;
        }

        internal class GetUrlInfo
        {
            public GetUrlInfo()
            {
                QueryStrings = new Dictionary<string, string>();
            }

            public string Url { get; set; }
            public string Action { get; set; }
            public string Controller { get; set; }
            public string Area { get; set; }

            public IDictionary<string, string> QueryStrings { get; set; }
        }

    }
}