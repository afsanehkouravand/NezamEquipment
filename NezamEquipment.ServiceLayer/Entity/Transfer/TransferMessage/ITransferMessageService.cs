using System;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Transfer.TransferMessage.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.TransferMessage
{
    public interface ITransferMessageService
    {
        DomainClasses.Entity.Transfer.TransferMessage New(string message);

        Task<TransferMessageDto> GetAsync(Guid? transferId = null,
            bool? suspension = null, bool? verification = null, TransferStep? step = null);

        Task<GetAllTupleResult<TransferMessageDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            Guid? transferId = null);
    }
}