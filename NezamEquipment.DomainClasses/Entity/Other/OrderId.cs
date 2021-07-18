using System;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;

namespace NezamEquipment.DomainClasses.Entity.Other
{
    public class OrderId
    {
        public long Id { get; set; }

        public int Code { get; set; }

        public OrderIdType Type { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

