using System;

namespace MyCommon.Helpers.Extension
{
    internal static class IsNullableEnumExtension
    {
        internal static bool IsNullableEnum(this Type t)
        {
            if (t.IsEnum)
                return true;
            
            var u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
    }
}
