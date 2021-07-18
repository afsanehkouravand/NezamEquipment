using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Entity.Employees.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Employees
{
    public interface IEmployeesService
    {
        Task<DbResult> AddAsync(
            EmployeDto dto);
      
        Task<DbResult> UpdateAsync(
            EmployeDto dto, string password = null, bool? registerSucces = null);
        Task<DbResult> UpdateAsync(Guid? id = null, string password = null, string nationalCode = null);
        Task<DbResult> DeleteAsync(Guid id);
        Task<EmployeDto> GetAsync(Guid? id = null, UnitType? unitType = null, string nationalCode = null,
            string? trackingCode = null);
        Task<GetAllTupleResult<EmployeDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            UnitType? unitType = null, string fullName = null, string nationalCode = null,
            IList<string> nationalCodes = null, IList<Guid> ids = null, int? codePersonal = null, string ip = null, NetworkType? networkType = null);
        Task<IEnumerable<EmployeDto>> GetAllNewAsync();
    }
}