using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;

namespace NezamEquipment.ServiceLayer.Entity.EquipmentFaulty
{
    public interface IEquipmentFaultyService
    {
        Task<DbResult> AddAsync(EquipmentFaultyDto dto, UploadByBase64Dto uploadByBase64);

        Task<DbResult> UpdateAsync(EquipmentFaultyDto dto);
        Task<DbResult> DeleteAsync(Guid id);
        Task<EquipmentFaultyDto> GetAsync(Guid? id = null, string code = null);

        Task<IEnumerable<EquipmentFaultyDto>> GetAllExcelAsync(int? skip = null,
            int? take = null, DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null);

        Task<int> GetAllExcelCountAsync( DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null);

        Task<GetAllTupleResult<EquipmentFaultyDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string fullName = null, string code = null,
            IList<Guid> ids = null , DateTime? fromCreatedOn = null, DateTime? toCreatedOn = null,Guid? equipmentId = null);
    }
}