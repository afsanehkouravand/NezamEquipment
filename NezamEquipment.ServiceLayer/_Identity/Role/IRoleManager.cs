using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NezamEquipment.DomainClasses.Identity.Enum;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.Role.Enum;
using Microsoft.AspNet.Identity;

namespace NezamEquipment.ServiceLayer._Identity.Role
{
    public interface IRoleManager
    {
        Task<DbResult> AddAsync(RoleDto dto);
        Task<DbResult> UpdateAsync(RoleDto dto);
        Task<DbResult> DeleteAsync(Guid roleId);
        Task<RoleDto> GetAsync(string name = null, Guid? id = null, AreaType? areaType = null);

        Task<IEnumerable<RoleDto>> GetAllAsync(Guid? userId = null, IList<string> rolesByName = null,
            IList<Guid> rolesById = null, AreaType? areaType = null);

        Task<IEnumerable<RoleDto>> GetAllDefaultRoleAsync();
        string GetDefaultRole(DefaultRole defaultRole);
        bool IsDefaultRole(string name);
        Task<DbResult> AddRoleAccessAsync(Guid roleId, List<RoleAccessDto> roleAccessDtos,
            string areaName = null, Guid? userId = null);
        Task<DbResult> DeleteRoleAccessAsync(Guid roleId);
        Task<List<RoleAccessDto>> GetAllRoleAccessAsync(Guid roleId);
        List<RoleAccessDto> GetAllRoleAccess(Guid roleId);
        Task<List<RoleAccessDto>> GetAllRoleAccessAsync(string area, string controller, string action);
        IList<RoleAccessDto> GetAllControllersNameThatTheUserHaveAccessTo(Guid userId);
        bool IsInRole(Guid userId, string roleName);
        void Dispose();
        Task<IdentityResult> CreateAsync(DomainClasses.Identity.Role role);
        Task<IdentityResult> UpdateAsync(DomainClasses.Identity.Role role);
        Task<IdentityResult> DeleteAsync(DomainClasses.Identity.Role role);
        Task<bool> RoleExistsAsync(string roleName);
        Task<DomainClasses.Identity.Role> FindByIdAsync(Guid roleId);
        Task<DomainClasses.Identity.Role> FindByNameAsync(string roleName);
        IIdentityValidator<DomainClasses.Identity.Role> RoleValidator { get; set; }
        IQueryable<DomainClasses.Identity.Role> Roles { get; }
    }
}