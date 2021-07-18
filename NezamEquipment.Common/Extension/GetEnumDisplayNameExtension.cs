using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NezamEquipment.Common.Extension
{
    public static class GetEnumDisplayNameExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetAttribute<DisplayAttribute>().Name;
        }

        private static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : System.Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

    }
}
