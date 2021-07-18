using System;
using System.Data.Entity.Infrastructure.Interception;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NezamEquipment.Common.DbCommand;
using NezamEquipment.Common.Extension;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml;
using NezamEquipment.Web.DNTScheduler;
using NezamEquipment.Web.Framework.ModelBinder;
using NezamEquipment.Web.Framework.Security;
using NezamEquipment.Web.Models;
using EFSecondLevelCache;
using PersianDataAnnotations;

namespace NezamEquipment.Web
{
    public static class Bootstrapping
    {
        public static void Start()
        {
            CheckSettings();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine()
            {
                AreaViewLocationFormats = new[]
                {
                        "~/Areas/{2}/Features/{1}/{0}.cshtml",
                        "~/Areas/{2}/Features/Shared/{0}.cshtml",
                    },
                AreaMasterLocationFormats = new[]
                {
                        "~/Areas/{2}/Features/{1}/{0}.cshtml",
                        "~/Areas/{2}/Features/Shared/{0}.cshtml",
                    },
                AreaPartialViewLocationFormats = new[]
                {
                        "~/Areas/{2}/Features/{1}/{0}.cshtml",
                        "~/Areas/{2}/Features/Shared/{0}.cshtml",
                    },
                FileExtensions = new[]
                {
                        "cshtml",
                    },
            });

            DbInterception.Add(new YeKeInterceptor());
            ScheduledTasksRegistry.Init();
            PersianDataAnnotations.PersianDataAnnotations.Register();

            //ModelBinders.Binders.DefaultBinder = new PersianModelBinder();
            //ModelBinders.Binders.Add(typeof(string), new PersianStringModelBinder());
            //ModelBinders.Binders.Add(typeof(DateTime), new PersianDateModelBinder());
            //ModelBinders.Binders.Add(typeof(DateTime?), new PersianDateModelBinder());

            // global info
            var settingService = DependencyResolver.Current.GetService<ISettingService>();
            GlobalInfo.LayoutTitle = settingService.Get<SettingOverAllXml>().Title;

            GetAllAreaControllerAction.GetInstance();

            new EFCacheServiceProvider().ClearAllCachedEntries();
        }

        private static void CheckSettings()
        {
            var folder = "~/App_Data/".MapPath();
            CheckPermission(folder);
            folder = "~/App_Data/Elmah".MapPath();
            CheckPermission(folder);
        }

        private static void CheckPermission(string folder)
        {
            if (!Directory.Exists(folder))
                throw new Exception(folder + " is not persist in current App.");
            var permission = new FileIOPermission(FileIOPermissionAccess.Write, folder);
            var permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.AddPermission(permission);
            if (!permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            {
                throw new Exception("You do not have a permission to Read/Write in " + folder);
            }
        }

    }
}