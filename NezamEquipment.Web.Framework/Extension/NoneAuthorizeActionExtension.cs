using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer.OtherServices.Cache;
using NezamEquipment.Web.Framework.Attribute;

namespace NezamEquipment.Web.Framework.Extension
{
    public static class NoneAuthorizeActionExtension
    {
        public static bool Check(string fullname, bool fromCache = true)
        {
            List<string> list;

            if (fromCache)
            {
                var cacheService = DependencyResolver.Current.GetService<ICacheService>();
                const string key = "AllActionWithNoneAuthorizeActionAttributeForMarking";
                if (cacheService.IsInCache(key))
                {
                    list = (List<string>) cacheService.GetFromCache(key);
                }
                else
                {
                    list = GetAllNoneAuthorizeAction();
                    cacheService.SaveToCache(key, list, DateTime.Now.AddYears(1));
                }
            }
            else
            {
                list = GetAllNoneAuthorizeAction();
            }

            return list.Contains(fullname.ToLower());
        }

        private static List<string> GetAllNoneAuthorizeAction()
        {
            var currentAssembly = AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == "NezamEquipment.Web")?.GetTypes();

            if (currentAssembly != null)
            {
                return currentAssembly
                    .Where(
                        type => typeof(Controller).IsAssignableFrom(type) && !type.Name.StartsWith("T4MVC_"))
                    .SelectMany(type => type.GetMethods())
                    .Where(method => method.IsPublic && method.IsDefined(typeof(NoneAuthorizeActionAttribute)))
                    .Select(
                        x =>
                            x.DeclaringType != null
                                ? x.DeclaringType.FullName.ToLower() + "." + x.Name.ToLower()
                                : null)
                    .ToList();
            }

            return new List<string>();
        }
    }
}