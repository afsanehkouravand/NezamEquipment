using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NezamEquipment.Common.Extension;
using NezamEquipment.Web.Controllers;
using NezamEquipment.Web.DNTScheduler;
using hbehr.recaptcha;

namespace NezamEquipment.Web
{
    public class MvcApplication : HttpApplication
    {

        #region Application_Start

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                Bootstrapping.Start();
                //var publicKey = ConfigurationManager.AppSettings["ReCaptcha.SiteKey"];
                //var secretKey = ConfigurationManager.AppSettings["ReCaptcha.SecretKey"];
                //ReCaptcha.Configure(publicKey, secretKey);
                ////Auto-select language from System.Thread.CurrentCulture
                //ReCaptcha.Configure(publicKey, secretKey, ReCaptchaLanguage.Persian);
            }
            catch (Exception ex)
            {
                throw ex;
                //ex.LogErrorForElmah();
                //HttpRuntime.UnloadAppDomain(); // سبب ری استارت برنامه و آغاز مجدد آن با درخواست بعدی می‌شود
            }
        }

        #endregion

        #region Application_End

        protected void Application_End()
        {
            //ScheduledTasksRegistry.End();
            //// This method needs a ping service to keep it alive.
            //ScheduledTasksRegistry.WakeUp(ConfigurationManager.AppSettings["SiteRootUrl"]);
        }

        #endregion

        #region Application_Error

        protected void Application_Error(object sender, EventArgs e)
        {
            //if (!HttpContext.Current.Request.IsLocal)
            //    SetCustomError();
        }

        #endregion

        #region Application_PreSendRequestHeaders

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //var app = sender as HttpApplication;
            //if (app == null || !app.Request.IsLocal || app.Context == null)
            //    return;
            //var headers = app.Context.Response.Headers;
            //headers.Remove("Server");
        }

        #endregion

        #region Private SetCustomError

        private void SetCustomError()
        {
            Response.Clear();

            var exception = Server.GetLastError();
            var httpException = exception as HttpException;

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            if (httpException == null)
                routeData.Values.Add("action", "Index");

            else //It's an Http Exception, Let's handle it.
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "HttpError404");
                        break;

                    default:
                        routeData.Values.Add("action", "Index");
                        break;
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("error", exception);

            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            Response.AddHeader("Content-Type", "text/html; charset=utf-8");

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(
                new HttpContextWrapper(Context), routeData));
        }

        #endregion

    }
}
