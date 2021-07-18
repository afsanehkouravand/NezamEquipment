using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Transfer.ApplicantTransfer.Dto;
using NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.Transfer
{
    public interface ITransferService
    {
        Task<DbResult> AddAReportFilesync(Guid transferId, UploadByBase64Dto uploadByBase64Dto);
        Task<DbResult> AddAsync(TransferDto transferDto, List<ApplicantTransferDto> applicantTransferDtos, IList<UploadByBase64Dto> uploadByBase64Dtos);
        Task<DbResult> ReSendAsync(Guid id, string message);
        Task<DbResult> SendAsync(Guid id, string address=null);
        Task<DbResult> EditAsync(TransferDto dto);
        Task<DbResult> UpdateAsync(Guid id, TransferStatusNew transferStatus);
        Task<DbResult> UpdateFinancialAsync(TransferDto dto);
        Task<DbResult> UpdateAsync(Guid id, TransferStepNew step, string message, TransferStepNew stepOrginal,
            bool suspension, bool verification, TransferStepNew? returnStep = null, TransferStepNew? gotoStep = null,
            bool? sendSms = null, bool? addMessage = null, TransferStepNew? suspensionStep = null, TransferStatusNew? transferStatus = null);

        Task<DbResult> UpdateArenaAndLandAsync(TransferDto transferDto);
        Task<GetAllTupleResult<TransferDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null, TransferStepNew? step = null, TransferStatusNew? status = null,
            Guid? id = null, IList<Guid> ids = null, Guid? applicantId = null, DateTime? fromCreatedOn = null, bool? isSuspension = null,
            DateTime? createdOn = null, DateTime? toCreatedOn = null, TransferStatusNew? notstep = null, bool? isSuspensionStep = null
            , string fromCreatedOnstr = null, string toCreatedOnstr = null);

        Task<GetAllTupleResult<TransferDto>> GetAllExtendedLicenseAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null, string registerPlak = null, TransferUserType? transferUserType = null,
            Guid? id = null, IList<Guid> ids = null, DateTime? fromCreatedOn = null,
            DateTime? createdOn = null, DateTime? toCreatedOn = null,
            string fromCreatedOnstr = null, string toCreatedOnstr = null);
         Task<GetAllTupleResult<TransferDto>> GetAllResendAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null,
            Guid? id = null, IList<Guid> ids = null, Guid? applicantId = null, DateTime? fromCreatedOn = null,
            DateTime? createdOn = null, DateTime? toCreatedOn = null);

        Task<TransferDto> GetAsync(Guid? id = null, IList<Guid> ids = null, Guid? applicantId = null, DateTime? fromCreatedOn = null, DateTime? createdOn = null,
            DateTime? toCreatedOn = null, string contractNumber = null);

        Task<DbResult> DeleteAsync(Guid id);

        Task<GetAllTupleResult<TransferDto>> GetAllContractNumberAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null);
    }
}