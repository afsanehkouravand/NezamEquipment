using System.Collections.Generic;
using System.Linq;

namespace NezamEquipment.Common.Extension
{
    public static class ListChunkByExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
