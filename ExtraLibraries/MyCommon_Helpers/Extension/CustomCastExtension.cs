using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Extension
{
    internal static class CustomCastExtension
    {
        internal static RoleAccessDto CustomCastToRoleAccess(this object myobj)
        {
            var x = new RoleAccessDto();

            var targetType = x.GetType();
            var d = from source in targetType.GetMembers().ToList()
                where source.MemberType == MemberTypes.Property
                select source;

            var memberInfos = d as IList<MemberInfo> ?? d.ToList();

            var members = memberInfos.Where(memberInfo => memberInfos.Select(c => c.Name).ToList().Contains(memberInfo.Name)).ToList();

            foreach (var memberInfo in members)
            {
                var propertyInfo = targetType.GetProperty(memberInfo.Name);
                var property = myobj.GetType().GetProperty(memberInfo.Name);
                if (property != null)
                {
                    if (propertyInfo != null)
                        propertyInfo.SetValue(x, property.GetValue(myobj, null), null);
                }
            }

            return x;
        }

    }
}
