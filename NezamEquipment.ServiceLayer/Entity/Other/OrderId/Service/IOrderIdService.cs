using System.Threading.Tasks;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;

namespace NezamEquipment.ServiceLayer.Entity.Other.OrderId.Service
{
    public interface IOrderIdService
    {
        Task<int> Get(OrderIdType type);
    }
}