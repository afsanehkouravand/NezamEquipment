using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;

namespace NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile
{
    public interface IEquipmentFaultyFileService
    {
        Task<DbResult> AddAsync(Guid applicantId, Guid equipmentFaultyId, IList<UploadByBase64Dto> uploadByBase64Dtos);
        Task<DbResult> UpdateAsync(Guid equipmentFaultyId, Guid id, IList<UploadByBase64Dto> uploadByBase64Dtos);
        Task<DbResult> UpdateFileAsync(Guid id, EquipmentFaultyFileStatus status, UploadByBase64Dto uploadByBase64Dto );
        Task<DbResult> UpdateAsync(Guid id, EquipmentFaultyFileStatus status);
        Task<DbResult> DeleteAsync(Guid id);

        Task<GetAllTupleResult<EquipmentFaultyFileDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            Guid? EquipmentFaultyFileId = null, EquipmentFaultyFileStatus? status=null,Guid? EquipmentFaultyId=null);

        Task<IEnumerable<EquipmentFaultyFileDto>> GetFileAsync(
            Guid? EquipmentFaultyFileId = null, EquipmentFaultyFileStatus? status = null, Guid? EquipmentFaultyId = null);

        Task<EquipmentFaultyFileDto> GetAsync(Guid? id = null,Guid? EquipmentFaultyId=null);
        List<EquipmentFaultyDto> GetListAsync(GetAllTupleDto getAllTupleDto = null, bool? all = null);
    }
}