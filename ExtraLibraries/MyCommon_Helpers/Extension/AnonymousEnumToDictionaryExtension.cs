using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyCommon.Helpers.Extension
{
    internal static class AnonymousEnumToDictionaryExtension
    {
        internal static Dictionary<int, string> AnonymousEnumToDictionary(this PropertyInfo propertyInfo)
        {
            var mydic = new Dictionary<int, string>();

            var type = propertyInfo.PropertyType;
            var nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType == null)
            {
                foreach (var foo in System.Enum.GetValues(type))
                {
                    var enumObject = (System.Enum)System.Enum.Parse(propertyInfo.PropertyType, foo.ToString());
                    var displayName = enumObject.GetDisplayName();
                    mydic.Add((int)foo, displayName);
                }
            }
            else
            {
                foreach (var foo in System.Enum.GetValues(nullableType))
                {
                    var enumObject = (System.Enum)System.Enum.Parse(nullableType, foo.ToString());
                    var displayName = enumObject.GetDisplayName();
                    mydic.Add((int)foo, displayName);
                }
            }

            return mydic;
        }
    }
}
