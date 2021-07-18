using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DomainClasses.Identity.Enum;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace NezamEquipment.ServiceLayer._Identity.User
{
    public class UserManager
        : UserManager<DomainClasses.Identity.User, Guid>, IUserManager
    {
        private readonly IIdentity _identity;
        private readonly IMapper _mapper;
        private readonly IRoleManager _roleManager;

        public UserManager(
            IUserStore<DomainClasses.Identity.User, Guid> store,
            IIdentity identity,
            IDataProtectionProvider dataProtectionProvider,
            IIdentityMessageService smsService,
            IIdentityMessageService emailService,
            IMapper mapper, 
            IRoleManager roleManager)
            : base(store)
        {
            _identity = identity;
            _mapper = mapper;
            _roleManager = roleManager;
            var dataProtectionProvider1 = dataProtectionProvider;
            SmsService = smsService;
            EmailService = emailService;

            #region createApplicationUserManager

            // Configure validation logic for usernames
            UserValidator = new UserValidator<DomainClasses.Identity.User, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            //RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser, int>
            //{
            //    MessageFormat = "Your security code is: {0}"
            //});
            //RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser, int>
            //{
            //    Subject = "SecurityCode",
            //    BodyFormat = "Your security code is {0}"
            //});

            if (dataProtectionProvider1 != null)
            {
                var dataProtector = dataProtectionProvider1.Create("ASP.NET Identity");
                UserTokenProvider = new DataProtectorTokenProvider<DomainClasses.Identity.User, Guid>(dataProtector);
            }

            #endregion

        }

        public Guid GetCurrentUserId()
        {
            var userId = _identity.GetUserId();
            if (userId != null)
                return Guid.Parse(userId);

            return Guid.Empty;
        }
     
        public UserDto Get(Guid? id = null, AreaType? areaType = null)
        {
            var e = Users.AsNoTracking().AsQueryable();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();

            if (areaType != null)
                e = e.Where(x => x.AreaType == areaType).AsQueryable();

            return e.ProjectTo<UserDto>(_mapper.ConfigurationProvider).Cacheable().FirstOrDefault();
        }

        public async Task<UserDto> GetAsync(Guid? id = null, AreaType? areaType = null, string userName = null)
        {
            var e = Users.AsNoTracking().AsQueryable();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();

            if (userName != null)
                e = e.Where(x => x.UserName.ToLower() == userName.ToLower()).AsQueryable();

            if (areaType != null)
                e = e.Where(x => x.AreaType == areaType).AsQueryable();

            return await e.ProjectTo<UserDto>(_mapper.ConfigurationProvider).Cacheable().FirstOrDefaultAsync();
        }

        public async Task<DbResult> DeleteAsync(Guid userId, AreaType? areaType = null)
        {
            var user = await Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                return new DbResult(DbResult.M.NotFound);

            if (areaType != null && user.AreaType != areaType)
                return new DbResult(DbResult.M.NotFound);

            var result = await DeleteAsync(user);
            if (!result.Succeeded)
                return new DbResult(result.Errors.First());
            
            return new DbResult();
        }

        public async Task<DbResult> UpdateAsync(UserDto userDto, IList<string> selectedRoles = null)
        {
            if (!string.IsNullOrWhiteSpace(userDto.Password))
                if (userDto.Password.Length < 6)
                    return new DbResult("رمز عبور نمی تواند کمتر از 6 حرف باشد.");
            
            var user = await Users.Where(x => x.Id == userDto.Id).FirstOrDefaultAsync();
            if (user == null)
                return new DbResult(DbResult.M.NotFound);

            user.Email = userDto.Email;
            user.UserName = userDto.UserName;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Position = userDto.Position;
            user.AreaType = userDto.AreaType;

            var resultUpdateUserEntity = await UpdateAsync(user);
            if (!resultUpdateUserEntity.Succeeded)
                return new DbResult(resultUpdateUserEntity.Errors.First());

            try
            {
                if (!string.IsNullOrWhiteSpace(userDto.Password))
                {
                    if ((await HasPasswordAsync(user.Id)))
                        await ChangePasswordAsync(user.Id, userDto.Password);
                    else
                        await AddPasswordAsync(user.Id, userDto.Password);
                }

                if (selectedRoles != null && selectedRoles.Any())
                {
                    var allRolesForUser = await GetRolesAsync(user.Id);
                    await AddToRolesAsync(user.Id, selectedRoles.Except(allRolesForUser).ToArray());
                    await RemoveFromRolesAsync(user.Id, allRolesForUser.Except(selectedRoles).ToArray());
                }
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
            }

            return new DbResult();
        }

        public async Task<DbResult> UpdateAsync(Guid userId, DateTime lastLogin)
        {
            var user = await Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                return new DbResult(DbResult.M.NotFound);

            user.LastLogin = lastLogin;

            var result = await UpdateAsync(user);
            if (!result.Succeeded)
                return new DbResult(result.Errors.First());

            return new DbResult();
        }

        public async Task<bool> AddAsync(UserDto userDto, IList<string> selectedRoles)
        {
            var user = _mapper.Map<DomainClasses.Identity.User>(userDto);
            user.Id = Guid.NewGuid();

            var result = await CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
                return false;

            selectedRoles = selectedRoles ?? new List<string>();
            await AddToRolesAsync(user.Id, selectedRoles.ToArray());

            return true;
        }

        public async Task<GetAllTupleResult<UserDto>> GetAllWithRolesAsync(GetAllTupleDto getAllTupleDto = null,
            string username = null, IList<Guid> ids = null, Guid? roleId = null, List<string> usernames = null, 
            AreaType? areaType = null, IList<Guid> roleIds = null, string position = null, string fullname = null)
        {
            var e = Users.AsNoTracking().Select(x => new UserDto
            {
                Id = x.Id,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Fullname = x.FirstName + " " + x.LastName,
                Position = x.Position,
                AreaType = x.AreaType,
                Email = x.Email,
                LastLogin = x.LastLogin,
                Roles = x.Roles.Select(c => new RoleDto
                {
                    Id = c.RoleId
                }).ToList(),
            }).OrderBy(x => x.Id).AsQueryable();

            if (roleIds != null && roleIds.Any())
                e = e.Where(x => x.Roles.Any(c => roleIds.Any(v => v == c.Id))).AsQueryable();

            if (!string.IsNullOrWhiteSpace(fullname))
                e = e.Where(x => x.Fullname.ToLower().Contains(fullname.ToLower())).AsQueryable();

            if (!string.IsNullOrWhiteSpace(position))
                e = e.Where(x => x.Position.ToLower().Contains(position.ToLower())).AsQueryable();

            if (areaType != null)
                e = e.Where(x => x.AreaType == areaType).AsQueryable();

            if (ids != null && ids.Any())
                e = e.Where(x => ids.Contains(x.Id)).AsQueryable();

            if (usernames != null && usernames.Any())
                e = e.Where(x => usernames.Contains(x.UserName)).AsQueryable();

            if (!string.IsNullOrEmpty(username))
                e = e.Where(x => x.UserName.ToLower().Contains(username.ToLower())).AsQueryable();

            if (roleId != null)
                e = e.Where(x => x.Roles.Any(c => c.Id == roleId)).AsQueryable();

            var result = await e.ToGetAllTupleResult<UserDto, UserDto>(getAllTupleDto, _mapper);

            var userList = result.List;

            var rolsIds = (from userWithRoleDto in userList from rols in userWithRoleDto.Roles select rols.Id).ToList();

            var roles = await _roleManager.GetAllAsync(rolesById: rolsIds);

            var roleDtos = roles as IList<RoleDto> ?? roles.ToList();

            foreach (var userWithRoleDto in userList)
            {
                foreach (var roleDto in userWithRoleDto.Roles)
                {
                    if (roleDtos.Any(c=>c.Id == roleDto.Id))
                    {
                        roleDto.Name = roleDtos.Where(x => x.Id == roleDto.Id).Select(x => x.Name).FirstOrDefault();
                    }
                }
            }

            return result;
        }

        public async Task<GetAllTupleResult<UserDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string username = null, IList<Guid> ids = null, List<string> usernames = null,
            AreaType? areaType = null, string position = null, string fullname = null)
        {
            var e = Users.AsNoTracking().Select(x => new UserDto
            {
                Id = x.Id,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Fullname = x.FirstName + " " + x.LastName,
                Position = x.Position,
                AreaType = x.AreaType,
                Email = x.Email,
            }).OrderBy(x => x.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(fullname))
                e = e.Where(x => x.Fullname.ToLower().Contains(fullname.ToLower())).AsQueryable();

            if (!string.IsNullOrWhiteSpace(position))
                e = e.Where(x => x.Position.ToLower().Contains(position.ToLower())).AsQueryable();

            if (areaType != null)
                e = e.Where(x => x.AreaType == areaType).AsQueryable();

            if (ids != null && ids.Any())
                e = e.Where(x => ids.Contains(x.Id)).AsQueryable();

            if (usernames != null && usernames.Any())
                e = e.Where(x => usernames.Contains(x.UserName)).AsQueryable();

            if (!string.IsNullOrEmpty(username))
                e = e.Where(x => x.UserName.ToLower().Contains(username.ToLower())).AsQueryable();

            return await e.ToGetAllTupleResult<UserDto, UserDto>(getAllTupleDto, _mapper);
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword)
        {
            var resultRemovePassword = await RemovePasswordAsync(userId);
            if (!resultRemovePassword.Succeeded)
                return false;

            var result = await AddPasswordAsync(userId, newPassword);
            return result.Succeeded;
        }
        //public async Task<bool> IsInRoleAsync(Guid userId, string role)
        //{
        //    var e = User.AsNoTracking().AsQueryable();


        //    return true;
        //}
    }
}