using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Identity.Enum;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using Microsoft.AspNet.Identity;

namespace NezamEquipment.ServiceLayer._Identity.User
{
    public interface IUserManager 
    {

        Guid GetCurrentUserId();
        UserDto Get(Guid? id = null, AreaType? areaType = null);

        Task<UserDto> GetAsync(Guid? id = null, AreaType? areaType = null, string userName = null);

        Task<DbResult> DeleteAsync(Guid userId, AreaType? areaType = null);

        Task<DbResult> UpdateAsync(UserDto user, IList<string> selectedRoles = null);

        Task<DbResult> UpdateAsync(Guid userId, DateTime lastLogin);

        Task<bool> AddAsync(UserDto user, IList<string> selectedRoles);

        Task<GetAllTupleResult<UserDto>> GetAllWithRolesAsync(GetAllTupleDto getAllTupleDto = null,
            string username = null, IList<Guid> ids = null, Guid? roleId = null, List<string> usernames = null,
            AreaType? areaType = null, IList<Guid> roleIds = null, string position = null, string fullname = null);

        Task<GetAllTupleResult<UserDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string username = null, IList<Guid> ids = null, List<string> usernames = null,
            AreaType? areaType = null, string position = null, string fullname = null);

        Task<bool> ChangePasswordAsync(Guid userId, string newPassword);

        #region Default

        Task<DomainClasses.Identity.User> FindByNameAsync(string userName);
        Task<IdentityResult> UpdateSecurityStampAsync(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string role);

        #endregion

    }
}