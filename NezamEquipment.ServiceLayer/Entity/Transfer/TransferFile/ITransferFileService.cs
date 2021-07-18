using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto;
using NezamEquipment.ServiceLayer.Entity.Transfer.TransferFile.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.TransferFile
{
    public interface ITransferFileService
    {
        Task<DbResult> AddAsync(Guid applicantId, Guid id, IList<UploadByBase64Dto> uploadByBase64Dtos);
        Task<DbResult> UpdateFileAsync(Guid id, TransferFileStatus status, UploadByBase64Dto uploadByBase64Dto);
        Task<DbResult> UpdateAsync(Guid id, TransferFileStatus status);
        Task<DbResult> UpdateAsync(Guid applicantId, Guid id, IList<UploadByBase64Dto> uploadByBase64Dtos);
        Task<DbResult> DeleteAsync(Guid id);
        Task<TransferFileDto> GetAsync(Guid? id = null, Guid? transferId = null);
        Task<GetAllTupleResult<TransferFileDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            Guid? transferFileId = null, TransferFileStatus? status=null, Guid? transferId = null);

        Task<IEnumerable<TransferFileDto>> GetFileAsync(
            Guid? transferFileId = null, TransferFileStatus? status = null, Guid? transferId = null);
        List<TransferDto> GetListAsync(GetAllTupleDto getAllTupleDto = null, bool? all = null);
    }
}