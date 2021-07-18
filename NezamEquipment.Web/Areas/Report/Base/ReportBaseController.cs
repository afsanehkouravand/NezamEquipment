using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer.Entity.Report.StimulReport;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml;
using NezamEquipment.Web.Framework.Base;

using Stimulsoft.Report.Mvc;

namespace NezamEquipment.Web.Areas.Report.Base
{
    public partial class ReportBaseController : BaseController
    {
        public Dictionary<string, string> StimulReportData { get; set; }

        public ReportBaseController(){}

        #region OnActionExecuting

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = new UrlHelper(HttpContext.Request.RequestContext);

            if (RouteData.Values["id"] == null)
            {
                filterContext.Result = new RedirectResult(url.Action(MVC.Error.NoPermission()));
                return;
            }

            var stimulReportService = DependencyResolver.Current.GetService<IStimulReportService>();

            var stimulReport = stimulReportService.Get(id: RouteData.Values["id"]);
            if (stimulReport == null)
            {
                filterContext.Result = new RedirectResult(url.Action(MVC.Error.NoPermission()));
                return;
            }

            var ifleft = (DateTime.Now - stimulReport.CreatedOn).TotalSeconds;
            var ifright = int.Parse(ConfigurationManager.AppSettings["StimulValidateLinkPerMinute"]) * 60;

            if (ifleft > ifright)
            {
                filterContext.Result = new RedirectResult(url.Action(MVC.Error.NoPermission()));
                return;
            }

            StimulReportData = stimulReport.DataDictionary;
        }

        #endregion

        #region OnActionExecuted

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Request.IsAjaxRequest())
                return;

            var settingService = DependencyResolver.Current.GetService<ISettingService>();

            var viewResult = filterContext.Result as ViewResultBase;
            var model = (ReportBaseViewModel)viewResult?.ViewData.Model;
            if (model != null)
            {
                model.LayoutTitle = settingService.Get<SettingOverAllXml>().Title;
            }
        }

        #endregion

        #region Stimul

        /// <summary>
        /// ایجاد پرینت
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult(HttpContext);
        }

        /// <summary>
        /// ایجاد خروجی
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult(HttpContext);
        }

        public virtual ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult(HttpContext);
        }

        public virtual ActionResult Interaction()
        {
            return StiMvcViewer.InteractionResult(HttpContext);
        }

        #endregion

    }
}