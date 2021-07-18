using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using Microsoft.AspNet.Identity;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.DomainClasses.Identity.Enum;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.Role.Enum;

namespace NezamEquipment.ServiceLayer._Identity.Role
{
    public class RoleManager : RoleManager<DomainClasses.Identity.Role, Guid>, IRoleManager
    {
        private readonly Dictionary<DefaultRole, string> _defaultRoles = new Dictionary<DefaultRole, string>
        {
            {DefaultRole.Admin, ConfigurationManager.AppSettings["Role.Admin"]},
            {DefaultRole.Banned, ConfigurationManager.AppSettings["Role.Banned"]},
          
        };

        private readonly IDbSet<RoleAccess> _roleAccesses;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _uow;
        private readonly IMapper _mapper;

        public RoleManager(
            IUnitOfWork<NezamEquipmentDbContext> uow,
            IRoleStore<DomainClasses.Identity.Role, Guid> roleStore,
            IMapper mapper) : base(roleStore)
        {
            _uow = uow;
            _mapper = mapper;
            _roleAccesses = uow.Set<RoleAccess>();
        }

        #region RoleDto Async

        public async Task<DbResult> AddAsync(RoleDto dto)
        {
            var exist = await GetAsync(name: dto.Name);
            if (exist != null)
                return new DbResult(DbResult.M.AlreadyExist);

            var role = _mapper.Map<DomainClasses.Identity.Role>(dto);
            role.Id = Guid.NewGuid();

            var result = await CreateAsync(role);
            if (!result.Succeeded)
                return new DbResult(DbResult.M.CanNotSave);

            return new DbResult();
        }

        public async Task<DbResult> UpdateAsync(RoleDto dto)
        {
            var role = await Roles.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (role == null)
                return new DbResult(DbResult.M.NotFound);

            role.Name = dto.Name;
            role.AreaType = dto.AreaType;

            var result = await UpdateAsync(role);
            if (!result.Succeeded)
                return new DbResult(DbResult.M.CanNotSave);

            return new DbResult();
        }

        public async Task<DbResult> DeleteAsync(Guid roleId)
        {
            var role = await Roles.Where(x => x.Id == roleId).FirstOrDefaultAsync();
            if (role == null)
                return new DbResult(DbResult.M.NotFound);

            var result = await DeleteAsync(role);
            if (!result.Succeeded)
                return new DbResult(DbResult.M.CanNotSave);

            return new DbResult();
        }
       

        public async Task<RoleDto> GetAsync(string name = null, Guid? id = null, AreaType? areaType = null)
        {
            var e = Roles.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                e = e.Where(x => x.Name == name).AsQueryable();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();

            if (areaType != null)
                e = e.Where(x => x.AreaType == areaType).AsQueryable();

            return await e.ProjectTo<RoleDto>(_mapper.ConfigurationProvider).Cacheable().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync(Guid? userId = null, IList<string> rolesByName = null,
            IList<Guid> rolesById = null, AreaType? areaType = null)
        {
            var e = Roles.AsNoTracking().AsQueryable();

            if (areaType != null)
                e = e.Where(x => x.AreaType == areaType).AsQueryable();

            if (userId != null)
                e = e.Where(x => x.Users.Any(c => c.UserId == userId)).AsQueryable();

            if (rolesByName != null && rolesByName.Any())
                e = e.Where(x => rolesByName.Any(c => c.ToLower() == x.Name.ToLower())).AsQueryable();

            if (rolesById != null && rolesById.Any())
                e = e.Where(x => rolesById.Any(c => c == x.Id)).AsQueryable();

            return await e.ProjectTo<RoleDto>(_mapper.ConfigurationProvider).Cacheable().ToListAsync();
        }

        #endregion

        #region DefaultRole

        public async Task<IEnumerable<RoleDto>> GetAllDefaultRoleAsync()
        {
            var listString = _defaultRoles.Select(x => x.Value.ToLower()).ToList();
            return
                await
                    Roles.AsNoTracking()
                        .Where(x => listString.Contains(x.Name.ToLower()))
                        .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                        .Cacheable()
                        .ToListAsync();
        }

        public string GetDefaultRole(DefaultRole defaultRole)
        {
            return _defaultRoles[defaultRole];
        }

        public bool IsDefaultRole(string name)
        {
            return _defaultRoles.Select(x => x.Value).Contains(name.ToLower());
        }

        #endregion

        #region RoleAccess

        public async Task<DbResult> AddRoleAccessAsync(Guid roleId, List<RoleAccessDto> roleAccessDtos,
            string areaName = null, Guid? userId = null)
        {
            if (roleAccessDtos == null || roleAccessDtos.All(x => x.HaveAccess == null))
                return new DbResult(DbResult.M.InCorrectAccess);

            foreach (var item in roleAccessDtos.Where(x=>x.HaveAccess != null).ToList())
            {
                _roleAccesses.Add(new RoleAccess
                {
                    RoleId = roleId,
                    CreateById = userId,
                    CreatedOn = DateTime.Now,
                    Area = areaName ?? item.Area,
                    Controller = item.Controller,
                    Action = item.Action,
                    HaveAccess = item.HaveAccess != null && item.HaveAccess.Value,
                });
            }

            var result = await _uow.SaveChangesAsync();
            if (result == 0)
                return new DbResult(DbResult.M.CanNotSave);

            return new DbResult();
        }

        public async Task<DbResult> DeleteRoleAccessAsync(Guid roleId)
        {
            var list = await _roleAccesses.Where(x => x.RoleId == roleId).ToListAsync();

            foreach (var item in list)
                _roleAccesses.Remove(item);

            var result = await _uow.SaveChangesAsync();
            if (result == 0)
                return new DbResult(DbResult.M.CanNotSave);

            return new DbResult();
        }

        public async Task<List<RoleAccessDto>> GetAllRoleAccessAsync(Guid roleId)
        {
            return
                await
                    _roleAccesses.AsNoTracking()
                        .Where(x => x.RoleId == roleId)
                        .ProjectTo<RoleAccessDto>(_mapper.ConfigurationProvider)
                        .Cacheable()
                        .ToListAsync();
        }

        public List<RoleAccessDto> GetAllRoleAccess(Guid roleId)
        {
            return
                _roleAccesses.AsNoTracking()
                    .Where(x => x.RoleId == roleId)
                    .ProjectTo<RoleAccessDto>(_mapper.ConfigurationProvider)
                    .Cacheable()
                    .ToList();
        }

        public async Task<List<RoleAccessDto>> GetAllRoleAccessAsync(string area, string controller, string action)
        {
            var roleAccess = await
                _roleAccesses.AsNoTracking()
                    .Where(x => x.Area == area && x.Controller == controller && x.Action == action)
                    .ProjectTo<RoleAccessDto>(_mapper.ConfigurationProvider)
                    .Cacheable()
                    .ToListAsync();

            var roleIds = roleAccess.Select(x => x.RoleId).ToList().Distinct().ToList();
            var roles = await Roles.Where(x => roleIds.Any(c => c == x.Id)).ToListAsync();

            foreach (var item in roleAccess)
            {
                item.RoleName = roles.First(x => x.Id == item.RoleId).Name;
            }

            return roleAccess;
        }

        #endregion

        #region HaveAccessToController

        public IList<RoleAccessDto> GetAllControllersNameThatTheUserHaveAccessTo(Guid userId)
        {
            var allRolesForUser = GetAll(userId).Select(x => x.Id).ToList();

            var getAllRoleAccessForAdminControllers = GetAllRoleAccess(roleIds: allRolesForUser);

            return getAllRoleAccessForAdminControllers;
        }  

        #endregion

        #region Other

        public bool IsInRole(Guid userId, string roleName)
        {
            return (GetAll(userId)).Any(x => x.Name == roleName);
        }
      
        #endregion

        #region Private

        private IEnumerable<RoleDto> GetAll(Guid? userId = null, IList<string> rolesByName = null, AreaType? areaType = null)
        {
            var e = Roles.AsNoTracking().AsQueryable();

            if (areaType != null)
                e = e.Where(x => x.AreaType == areaType).AsQueryable();

            if (userId != null)
                e = e.Where(x => x.Users.Any(c => c.UserId == userId)).AsQueryable();

            if (rolesByName != null)
                e = e.Where(x => rolesByName.Any(c => c.ToLower() == x.Name.ToLower())).AsQueryable();

            return e.ProjectTo<RoleDto>(_mapper.ConfigurationProvider).Cacheable().ToList();
        }

        private IList<RoleAccessDto> GetAllRoleAccess(IList<Guid> roleIds)
        {
            return
                _roleAccesses.AsNoTracking()
                        .Where(x => roleIds.Contains(x.RoleId))
                        .ProjectTo<RoleAccessDto>(_mapper.ConfigurationProvider)
                        .Cacheable()
                        .ToList();
        }

        #endregion

    }
}