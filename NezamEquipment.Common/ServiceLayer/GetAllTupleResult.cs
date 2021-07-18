using System.Collections.Generic;

namespace NezamEquipment.Common.ServiceLayer
{
    public class GetAllTupleResult<T> where T : class
    {
        public List<T> List { get; set; }

        public int Count { get; set; }

        public object ExtraData { get; set; }
    }
}
