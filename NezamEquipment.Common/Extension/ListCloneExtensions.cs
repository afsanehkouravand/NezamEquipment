using System.Collections.Generic;

namespace NezamEquipment.Common.Extension
{
    public static class ListCloneExtensions
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : class
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