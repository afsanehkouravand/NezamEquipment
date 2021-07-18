using System;

namespace NezamEquipment.Common.Extension
{
    public static class IsNullableEnumExtension
    {
        public static bool IsNullableEnum(this Type t)
        {
            if (t.IsEnum)
                return true;
            
            var u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
    }
}
