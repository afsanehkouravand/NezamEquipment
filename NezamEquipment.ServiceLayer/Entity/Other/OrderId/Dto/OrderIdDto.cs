using System;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;

namespace NezamEquipment.ServiceLayer.Entity.Other.OrderId.Dto
{
    public class OrderIdDto
    {
        public long Id { get; set; }

        public int Code { get; set; }

        public OrderIdType Type { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
