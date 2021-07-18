using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MyCommon.Helpers.Extension
{
    internal static class GetDisplayNameExtension
    {
        /// <summary>
        /// 
        /// </summary>
        internal static string GetDisplayName(this System.Enum enumValue)
        {
            return enumValue.GetAttribute<DisplayAttribute>().Name;
        }

        private static TAttribute GetAttribute<TAttribute>(this System.Enum enumValue)
        where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

    }
}
