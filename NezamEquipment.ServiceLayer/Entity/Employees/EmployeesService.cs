using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NezamEquipment.Common.Security;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Other.SmsLog;
using NezamEquipment.ServiceLayer.OtherServices.UtilityService;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using NezamEquipment.ServiceLayer.Entity.Employees.Dto;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.DomainClasses.Identity;

namespace NezamEquipment.ServiceLayer.Entity.Employees
{

    public class EmployeesService : BaseSaveDbResult, IEmployeesService
    {

        private readonly IDbSet<DomainClasses.Entity.Employees.Employe> _nezamEmployes;
        private readonly IDbSet<User> _user;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUtilityService _utilityService;
        private readonly IUserManager _userManager;
        public EmployeesService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper, IUtilityService utilityService, IUserManager userManager
          )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _utilityService = utilityService;
            _userManager = userManager;
            _nezamEmployes = _unitOfWork.Set<DomainClasses.Entity.Employees.Employe>();
            _user= _unitOfWork.Set<DomainClasses.Identity.User>(); 
        }

        public async Task<DbResult> AddAsync(
            EmployeDto dto)
        {
            ulong checkCode;
            if (!ulong.TryParse(dto.NationalCode, out checkCode))
            {
                return new DbResult("کد ملی وارد شده صحیح نمی باشد.");
            }
            if (!_utilityService.IsValidNationalCode(dto.NationalCode))
            {
                return new DbResult("کد ملی وارد شده صحیح نمی باشد.");
            }
            if (!_utilityService.CheckMobile(dto.MobileNumber))
            {
                return new DbResult("شماره موبایل وارد شده صحیح نمی باشد.");
            }

            var checkNezamEmploye = await GetAsync(nationalCode: dto.NationalCode);
            if (checkNezamEmploye != null)
            {
              
                return new DbResult(" کد ملی وارد شده از قبل در سیستم ثبت شده است .");
            }
            var applicant = _mapper.Map<DomainClasses.Entity.Employees.Employe>(dto);
            applicant.UserId = _userManager.GetCurrentUserId();
            //if (applicantDto.BirthDateStr != null)
            //{

            //    applicant.BirthDate = applicantDto.BirthDateStr.PersianNumberToEnglish().ToMiladiDate(false);
            //}

            //applicant.PasswordSalt = GenerateSaltedHash();
            //applicant.Password = applicant.Password.ToString().Md5Hash(applicant.PasswordSalt);


            _nezamEmployes.Add(applicant);

            var result = await SaveDbResult(_unitOfWork, applicant);
            
            return result;
        }

             public async Task<DbResult> UpdateAsync(
                 EmployeDto dto, string password = null, bool? registerSucces = null)
        {
            ulong checkCode;
            if (!ulong.TryParse(dto.NationalCode, out checkCode))
            {
                return new DbResult("کد ملی وارد شده صحیح نمی باشد.");
            }
            if (!_utilityService.IsValidNationalCode(dto.NationalCode))
            {
                return new DbResult("کد ملی وارد شده صحیح نمی باشد.");
            }
            if (!_utilityService.CheckMobile(dto.MobileNumber))
            {
                return new DbResult("شماره موبایل وارد شده صحیح نمی باشد.");
            }

            var applicant = await _nezamEmployes.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (applicant == null)
                return new DbResult(DbResult.M.NotFound);

            if (applicant.NationalCode != dto.NationalCode)
            {
                var checkNezamEmploye = await GetAsync(nationalCode: dto.NationalCode);
                if (checkNezamEmploye != null)
                {
                    return new DbResult(" کد ملی وارد شده از قبل در سیستم ثبت شده است .");
                }
            }

            applicant.unitType = dto.unitType;
            applicant.Fullname = dto.Fullname;
            applicant.NationalCode = dto.NationalCode;
            applicant.GenderType = dto.GenderType;
            applicant.NationalityType = dto.NationalityType;
            applicant.FatherName = dto.FatherName;
            applicant.PlaceOfBirth = dto.PlaceOfBirth;
            applicant.CertificateCode = dto.CertificateCode;
            applicant.PhoneNumber = dto.PhoneNumber;
            applicant.MobileNumber = dto.MobileNumber;
            applicant.Description = dto.Description;
            applicant.Address = dto.Address;
            applicant.PostalCode = dto.PostalCode;
            applicant.LoginStatus = dto.LoginStatus;
            applicant.IP = dto.IP;
            applicant.CodePersonal = dto.CodePersonal;
            applicant.NetworkType = dto.NetworkType;
            applicant.UserLogin = dto.UserLogin;


            applicant.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(password))
            {
                applicant.PasswordSalt = GenerateSaltedHash();
                applicant.Password = password.Md5Hash(applicant.PasswordSalt);
            }


            var result = await SaveDbResult(_unitOfWork, applicant);
            return result;

        }
    

        public async Task<DbResult> UpdateAsync(Guid? id = null, string password = null, string nationalCode = null)
        {
            var e = _nezamEmployes.AsQueryable();

            if (id != null)
                e = e.Where(x => x.Id == id);

            if (!string.IsNullOrWhiteSpace(nationalCode))
                e = e.Where(x => x.NationalCode == nationalCode);

            var applicant = await e.FirstOrDefaultAsync();
            if (applicant == null)
                return new DbResult(DbResult.M.NotFound);

            applicant.PasswordSalt = GenerateSaltedHash();
            applicant.Password = password.Md5Hash(applicant.PasswordSalt);
          
            _unitOfWork.MarkAsChanged(applicant);
            var result = await SaveDbResult(_unitOfWork, applicant);
          
            if (result.Status)

                return new DbResult(true);
            else
            {
                return new DbResult(false);
            }
        }

        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var applicant = await _nezamEmployes.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (applicant == null)
                return new DbResult(DbResult.M.NotFound);

            _unitOfWork.MarkAsSafeDelete(applicant);



            return await SaveDbResult(_unitOfWork);
        }

     
        public async Task<EmployeDto> GetAsync(Guid? id = null, UnitType? unitType = null, string nationalCode = null,
            string ? trackingCode = null)
        {
            var e = _nezamEmployes.AsNoTracking().AsQueryable();
          //var e = Dto();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();

            if (unitType != null)
                e = e.Where(x => x.unitType == unitType).AsQueryable();

            if (nationalCode != null)
                e = e.Where(x => x.NationalCode == nationalCode).AsQueryable();

           

            return await e.ProjectTo<EmployeDto>(_mapper.ConfigurationProvider).Cacheable().FirstOrDefaultAsync();
        }

        public async Task<GetAllTupleResult<EmployeDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            UnitType? unitType = null, string fullName = null, string nationalCode = null,
            IList<string> nationalCodes = null, IList<Guid> ids = null, int? codePersonal = null, string ip = null, NetworkType? networkType=null)
        {
            var e = _nezamEmployes.AsNoTracking().AsQueryable();

           
            if (unitType != null)
                e = e.Where(x => x.unitType == unitType).AsQueryable();

            if (codePersonal != null)
                e = e.Where(x => x.CodePersonal == codePersonal).AsQueryable();
            if (ip != null)
                e = e.Where(x => x.IP == ip).AsQueryable();

            if (networkType != null)
                e = e.Where(x => x.NetworkType == networkType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(fullName))
                e = e.Where(x => x.Fullname.ToLower().Contains(fullName.ToLower())).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nationalCode))
                e = e.Where(x => x.NationalCode.ToLower().Contains(nationalCode.ToLower())).AsQueryable();

            if (nationalCodes != null && nationalCodes.Any())
                e = e.Where(x => nationalCodes.Any(c => c == x.NationalCode)).AsQueryable();

            if (ids != null && ids.Any())
                e = e.Where(x => ids.Any(c => c == x.Id)).AsQueryable();

            return await e.ToGetAllTupleResult<DomainClasses.Entity.Employees.Employe, EmployeDto>(getAllTupleDto, _mapper);
        }

        public async Task<IEnumerable<EmployeDto>> GetAllNewAsync()
        {
            return await _nezamEmployes.AsNoTracking().Select(x => new EmployeDto()
            {
                Id = x.Id,
                Fullname = x.Fullname,

            }).ToListAsync();
        }
        private static byte[] GenerateSaltedHash()
        {
            const string plainTextForHash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*(){}|?><";

            var plainText = plainTextForHash.Select(Convert.ToByte).ToArray();
            var salt = ConfigurationManager.AppSettings["Encrypt"].Select(Convert.ToByte).ToArray();

            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
                new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

     
        private IQueryable<EmployeDto> Dto()
        {
            return (from employ in _nezamEmployes
                    join userinfo in _user on employ.UserId equals userinfo.Id

                    select new EmployeDto()
                    {
                        NationalCode = employ.NationalCode,
                        Fullname = employ.Fullname,
                        Address = employ.Address,
                        Description = employ.Description,
                        FatherName = employ.FatherName,
                        CreatedOn = employ.CreatedOn,
                        Id = employ.Id,
                        MobileNumber = employ.MobileNumber,
                        PhoneNumber = employ.PhoneNumber,
                        PlaceOfBirth = employ.PlaceOfBirth,
                        Email = employ.Email,
                        GenderType = employ.GenderType,
                        unitType = employ.unitType,
                        BirthDate = employ.BirthDate,
                        IP = employ.IP,
                        NetworkType = employ.NetworkType,
                        UserLogin = employ.UserLogin,
                        CodePersonal = employ.CodePersonal,
                        UserFullName = userinfo.FirstName + userinfo.LastName,

                    }).AsQueryable();

        }


    }
}
