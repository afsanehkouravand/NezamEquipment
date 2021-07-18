using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Equipment
{
    public interface IEquipmentService
    {
        Task<DbResult> AddAsync(
            EquipmentDto dto);
      
        Task<DbResult> UpdateAsync(EquipmentDto dto);
          Task<DbResult> DeleteAsync(Guid id);
        Task<EquipmentDto> GetAsync(Guid? id = null, UnitType? unitType = null, string code = null,
            EquipmentStatus? equipmentStatus = null);
        Task<IEnumerable<EquipmentDto>> GetAllExcelAsync(int? skip = null,
          int? take = null, DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null, EquipmentType? equipmentType = null,
            EquipmentStatus? equipmentStatus = null, UnitType? unitType = null) ;
        Task<int> GetAllExcelCountAsync(DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null, EquipmentType? equipmentType = null,
         EquipmentStatus? equipmentStatus = null, UnitType? unitType = null);
        Task<GetAllTupleResult<EquipmentDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            UnitType? unitType = null, string fullName = null, string code = null,
            IList<Guid> ids = null, EquipmentType? equipmentTypes = null, EquipmentStatus? equipmentStatus = null, DateTime? fromCreatedOn = null, DateTime? toCreatedOn = null, Guid? employId = null);
      
    }
}