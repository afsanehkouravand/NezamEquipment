using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.OtherServices.UtilityService;
using AutoMapper;
using EFSecondLevelCache;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.Common.Normalization;
using NezamEquipment.Common.Extension;

namespace NezamEquipment.ServiceLayer.Entity.Equipment
{

    public class EquipmentService : BaseSaveDbResult, IEquipmentService
    {

        private readonly IDbSet<DomainClasses.Entity.Equipment.Equipment> _nezamEquipments;
        private readonly IDbSet<DomainClasses.Entity.Employees.Employe> _employeeses;
        private readonly IDbSet<User> _user;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUtilityService _utilityService;
        private readonly IUserManager _userManager;

        public EquipmentService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper, IUtilityService utilityService, IUserManager userManager
          )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _utilityService = utilityService;
            _userManager = userManager;
            _nezamEquipments = _unitOfWork.Set<DomainClasses.Entity.Equipment.Equipment>();
            _employeeses = _unitOfWork.Set<DomainClasses.Entity.Employees.Employe>();
            ;
            _user = _unitOfWork.Set<DomainClasses.Identity.User>();
        }

        public async Task<DbResult> AddAsync(
            EquipmentDto dto)
        {
            if(dto.Code!="-" && dto.Code != null)
            {
                var checkNezamEquipment = await GetAsync(code: dto.Code);
                if (checkNezamEquipment != null)
                {

                    return new DbResult("این کد اموال از قبل در سیستم ثبت شده است.");
                }
            }
            

            var applicant = _mapper.Map<DomainClasses.Entity.Equipment.Equipment>(dto);
            applicant.UserId = _userManager.GetCurrentUserId();
            if(dto.unitType!=UnitType.None)
            {
                applicant.EmployeesId = dto.EmployeesId;
            }
         else
            {
                applicant.EmployeesId = null;
            }
            if (dto.DateBuystr != null)
            {

                applicant.DateBuy = dto.DateBuystr.PersianNumberToEnglish().ToMiladiDate(false);
            }
            _nezamEquipments.Add(applicant);

            var result = await SaveDbResult(_unitOfWork, applicant);

            return result;
        }

        public async Task<DbResult> UpdateAsync(EquipmentDto dto)
        {

            var applicant = await _nezamEquipments.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (applicant == null)
                return new DbResult(DbResult.M.NotFound);
            if (dto.Code != "-" && dto.Code != null)
            {
                if (applicant.Code != dto.Code)
                {
                    var checkNezamEquipment = await GetAsync(code: dto.Code);
                    if (checkNezamEquipment != null)
                    {
                        return new DbResult("این کد اموال از قبل در سیستم ثبت شده است.");
                    }
                }
            }
            
            if (dto.unitType == UnitType.None)
            {
                applicant.EmployeesId = null;
            }
            else
            {
                applicant.EmployeesId = dto.EmployeesId;
            }
            if (dto.DateBuystr != null)
            {

                applicant.DateBuy = dto.DateBuystr.PersianNumberToEnglish().ToMiladiDate(false);
            }
            
            applicant.UnitType = dto.unitType;
            applicant.Code = dto.Code;
            applicant.Model = dto.Model;
            applicant.EquipmentStatus = dto.EquipmentStatus;
            applicant.Brand = dto.Brand;
           
            applicant.EquipmentTypes = dto.EquipmentTypes;
            applicant.EquipmentTypes = dto.EquipmentTypes;
            applicant.Description = dto.Description;

            var result = await SaveDbResult(_unitOfWork, applicant);
            return result;

        }

        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var applicant = await _nezamEquipments.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (applicant == null)
                return new DbResult(DbResult.M.NotFound);

            _unitOfWork.MarkAsSafeDelete(applicant);



            return await SaveDbResult(_unitOfWork);
        }

        public async Task<EquipmentDto> GetAsync(Guid? id = null, UnitType? unitType = null, string code = null,
            EquipmentStatus? equipmentStatus = null)
        {
            //var e = _nezamEquipments.AsNoTracking().AsQueryable();
            var e = Dto();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();

            if (unitType != null)
                e = e.Where(x => x.unitType == unitType).AsQueryable();

            if (code != null)
                e = e.Where(x => x.Code == code).AsQueryable();
            if (equipmentStatus != null)
                e = e.Where(x => x.EquipmentStatus == equipmentStatus).AsQueryable();


            return await e.Cacheable().FirstOrDefaultAsync();
          
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllExcelAsync(int? skip = null,
          int? take = null, DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null, EquipmentType? equipmentType = null,
            EquipmentStatus? equipmentStatus = null,UnitType ? unitType = null)
        {
            var e = (
                     from equipment in _nezamEquipments
                     join employe in _employeeses on equipment.EmployeesId equals employe.Id into employeJoin
                     from employe in employeJoin.DefaultIfEmpty()
                     where equipment.IsDeleted == false
                     select new
                     {
                         equipment.Code,
                         equipment.Model,
                         equipment.EquipmentStatus,
                         equipment.DateBuy,
                         equipment.Brand,                       
                         unitType = equipment.UnitType,
                         equipment.EmployeesId,
                         equipment.Description,
                         equipment.CreatedOn,
                         equipment.EquipmentTypes,
                        

                     }).OrderBy(x => x.CreatedOn).AsQueryable();

           
            if (dateBuyFrom != null)
                e = e.Where(x => x.DateBuy >= dateBuyFrom).AsQueryable();

            if (dateBuyTo != null)
                e = e.Where(x => x.CreatedOn <= dateBuyTo).AsQueryable();

            if (equipmentStatus != null)
                e = e.Where(x => x.EquipmentStatus == equipmentStatus).AsQueryable();

            if (equipmentType != null)
                e = e.Where(x => x.EquipmentTypes == equipmentType).AsQueryable();

            if (unitType != null)
                e = e.Where(x => x.unitType == unitType).AsQueryable();


            if (skip != null && take != null)
                e = e.Skip((skip.Value - 1) * take.Value).AsQueryable();

            if (take != null)
                e = e.Take(take.Value).AsQueryable();



            return await e.Select(x => new EquipmentDto()
            {
                Code=x.Code,
                Model = x.Model,
                DateBuy = x.DateBuy,
                Brand = x.Brand,
                unitType = x.unitType,
                EmployeesId = x.EmployeesId,
                Description = x.Description,
                EquipmentTypes = x.EquipmentTypes,
                CreatedOn = x.CreatedOn,

            }).ToListAsync();
        }

        public async Task<int> GetAllExcelCountAsync( DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null, EquipmentType? equipmentType = null,
         EquipmentStatus? equipmentStatus = null, UnitType? unitType = null)
        {
          
            var e = (
                    from equipment in _nezamEquipments
                    join employe in _employeeses on equipment.EmployeesId equals employe.Id into employeJoin
                    from employe in employeJoin.DefaultIfEmpty()
                    where equipment.IsDeleted == false
                    select new
                    {
                        equipment.Code,
                        equipment.Model,
                        equipment.EquipmentStatus,
                        equipment.DateBuy,
                        equipment.Brand,
                        unitType = equipment.UnitType,
                        equipment.EmployeesId,
                        equipment.Description,
                        equipment.CreatedOn,
                        equipment.EquipmentTypes,

                    }).OrderBy(x => x.CreatedOn).AsQueryable();
                                    
                if (dateBuyFrom != null)
                e = e.Where(x => x.DateBuy >= dateBuyFrom).AsQueryable();

            if (dateBuyTo != null)
                e = e.Where(x => x.CreatedOn <= dateBuyTo).AsQueryable();

            if (equipmentStatus != null)
                e = e.Where(x => x.EquipmentStatus == equipmentStatus).AsQueryable();

            if (equipmentType != null)
                e = e.Where(x => x.EquipmentTypes == equipmentType).AsQueryable();

            if (unitType != null)
                e = e.Where(x => x.unitType == unitType).AsQueryable();

            return await e.Select(x => new EquipmentDto()
            {
                Code = x.Code,
                Model = x.Model,
                DateBuy = x.DateBuy,
                Brand = x.Brand,
                unitType = x.unitType,
                EmployeesId = x.EmployeesId,
                Description = x.Description,
                EquipmentTypes = x.EquipmentTypes,
                CreatedOn = x.CreatedOn,
              

            }).CountAsync();
        }
        public async Task<GetAllTupleResult<EquipmentDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            UnitType? unitType = null, string fullName = null, string code = null,
            IList<Guid> ids = null, EquipmentType? equipmentTypes = null, EquipmentStatus? equipmentStatus = null, DateTime? fromCreatedOn = null, DateTime? toCreatedOn = null,Guid? employId=null)
        {
            //var e = _nezamEquipments.AsNoTracking().AsQueryable();
            var e = Dto();

            if (unitType != null)
                e = e.Where(x => x.unitType == unitType).AsQueryable();

            if (code != null)
                e = e.Where(x => x.Code == code).AsQueryable();
            if (equipmentTypes != null)
                e = e.Where(x => x.EquipmentTypes == equipmentTypes).AsQueryable();
           
            if (equipmentStatus != null)
                e = e.Where(x => x.EquipmentStatus == equipmentStatus).AsQueryable();
            if (employId != null)
                e = e.Where(x => x.EmployeesId == employId).AsQueryable();
            if (fromCreatedOn != null)
            {
                var start = new DateTime(fromCreatedOn.Value.Year, fromCreatedOn.Value.Month, fromCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.DateBuy >= start).AsQueryable();

            }

            if (toCreatedOn != null)
            {
                var end = new DateTime(toCreatedOn.Value.Year, toCreatedOn.Value.Month, toCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.DateBuy < end).AsQueryable();
            }
            if (ids != null && ids.Any())
                e = e.Where(x => ids.Any(c => c == x.Id)).AsQueryable();
           return await e.ToGetAllTupleResult<EquipmentDto, EquipmentDto>(getAllTupleDto);
         
        }

        private IQueryable<EquipmentDto> Dto()
        {
            return (from equipment in _nezamEquipments
                    join employ in _employeeses on equipment.EmployeesId equals employ.Id into ps
                    from employ in ps.DefaultIfEmpty()
                    select new EquipmentDto()
                    {
                        Id = equipment.Id,
                        unitType = equipment.UnitType,
                        Code = equipment.Code,
                        Model = equipment.Model,
                        EquipmentStatus = equipment.EquipmentStatus,
                        Brand = equipment.Brand,
                        DateBuy = equipment.DateBuy,
                        EquipmentTypes = equipment.EquipmentTypes,
                        Description = equipment.Description,
                        EmployeesId = equipment.EmployeesId,
                        EmployeFullName = employ.Fullname,
                        //DateBuystr = ShamsiDateTimeExtension.ShamsiDateTime(equipment.DateBuy.Value, false),
                    }).AsQueryable();

        }


    }
}
