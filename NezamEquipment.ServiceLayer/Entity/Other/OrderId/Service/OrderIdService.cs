using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;

namespace NezamEquipment.ServiceLayer.Entity.Other.OrderId.Service
{
    public class OrderIdService : IOrderIdService
    {
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IDbSet<DomainClasses.Entity.Other.OrderId> _orderIds;


        public OrderIdService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderIds = _unitOfWork.Set<DomainClasses.Entity.Other.OrderId>();
        }

        public async Task<int> Get(OrderIdType type)
        {
            var code = "2" + (int)type;
            var number = new Random().Next(9999999).ToString().PadLeft(7, '0'); 

            var codeInt = int.Parse(code + number);

            if (await ExistAsync(codeInt, type))
            {
                return await Get(type);
            }

            await AddAsync(codeInt, type);

            return codeInt;
        }

        private async Task<DbResult> AddAsync(int code, OrderIdType type)
        {
            var orderId = new DomainClasses.Entity.Other.OrderId
            {
                CreatedOn = DateTime.Now,
                Code = code,
                Type = type,
            };

            _orderIds.Add(orderId);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 0)
                return new DbResult("امکان ذخیره اطلاعات وجود ندارد.");

            return new DbResult(true);
        }

        private async Task<bool> ExistAsync(int code, OrderIdType type)
        {
            return await _orderIds.AsNoTracking().Where(x => x.Code == code && x.Type == type).AnyAsync();
        }

    }
}
