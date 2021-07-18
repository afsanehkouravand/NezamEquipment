using System.Collections.Generic;

namespace MyCommon.Helpers.Extension
{
    internal static class ListCloneExtensions
    {
        internal static List<T> Clone<T>(this List<T> listToClone) where T : class
        {
            var list = new List<T>();
            foreach (var item in listToClone)
            {
                list.Add(item);
            }
            return list;
        }
    }
}