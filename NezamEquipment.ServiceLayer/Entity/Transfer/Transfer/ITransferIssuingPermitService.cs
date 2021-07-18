using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.Transfer
{
    public interface ITransferIssuingPermitService
    {
        Task<DbResult> AddAsync(TransferIssuingPermitDto transferIssuingPermitDto);
        Task<DbResult> EditAsync(TransferIssuingPermitDto dto);

        Task<GetAllTupleResult<TransferIssuingPermitDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null,
            Guid? id = null, IList<Guid> ids = null, DateTime? fromCreatedOn = null,
            DateTime? createdOn = null, DateTime? toCreatedOn = null);

        Task<IEnumerable<TransferIssuingPermitDto>> GetAllPermitReportAsync(bool isLast,
            int? skip = null,
            int? take = null, DateTime? dateFrom = null, DateTime? dateTo = null);

        Task<int> GetAllPermitReportCountAsync(bool hasCotazh, DateTime? dateFrom = null, DateTime? dateTo = null,
            string nameCompany = null);
        Task<TransferIssuingPermitDto> GetAsync(Guid? id = null,Guid? transferId = null, IList<Guid> ids = null, DateTime? fromCreatedOn = null, DateTime? createdOn = null,
            DateTime? toCreatedOn = null, string contractNumber = null);

        Task<DbResult> DeleteAsync(Guid id);

        Task<GetAllTupleResult<TransferIssuingPermitDto>> GetAllContractNumberAsync(
            GetAllTupleDto getAllTupleDto = null,
            string lisenceNumber = null);
    }
}