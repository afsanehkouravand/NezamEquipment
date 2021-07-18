using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.ServiceLayer.OtherServices.StateLog.Dto;

namespace NezamEquipment.ServiceLayer.OtherServices.StateLog.Service
{
    public interface IStateLogService
    {
        void Add(Guid transferId, TransferStatusNew stateLogStateCode, string stateLogTitle, string stateLogText);
        Task<DbResult> AddAsync(Guid transferId, TransferStatusNew stateLogStateCode, string stateLogTitle, string stateLogText);
        Task<DbResult> DeleteAsync(long id);
        Task<IEnumerable<StateLogDto>> GetAllAsync(Guid? transferId = null);
    }
}