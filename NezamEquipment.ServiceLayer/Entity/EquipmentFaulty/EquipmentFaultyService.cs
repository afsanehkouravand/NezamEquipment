using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using EFSecondLevelCache;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.Normalization;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Enum;
using NezamEquipment.ServiceLayer.OtherServices.UtilityService;


namespace NezamEquipment.ServiceLayer.Entity.EquipmentFaulty
{

    public class EquipmentFaultyService : BaseSaveDbResult, IEquipmentFaultyService
    {

        private readonly IDbSet<DomainClasses.Entity.EquipmentFaulty.EquipmentFaulty> _nezamEquipmentFaultys;
        private readonly IDbSet<DomainClasses.Entity.Equipment.Equipment> _equipments;
        private readonly IDbSet<User> _user;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUtilityService _utilityService;
        private readonly IUserManager _userManager;
        private readonly IUploadService _uploadService;
        public EquipmentFaultyService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper, IUtilityService utilityService, IUserManager userManager, IUploadService uploadService
          )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _utilityService = utilityService;
            _userManager = userManager;

            _nezamEquipmentFaultys = _unitOfWork.Set<DomainClasses.Entity.EquipmentFaulty.EquipmentFaulty>();
            _equipments = _unitOfWork.Set<DomainClasses.Entity.Equipment.Equipment>(); ;
            _user = _unitOfWork.Set<DomainClasses.Identity.User>();
            _uploadService = uploadService;
        }

        public async Task<DbResult> AddAsync(
            EquipmentFaultyDto dto, UploadByBase64Dto uploadByBase64)
        {
            dto.CreatedOn = DateTime.Now;
            dto.UserId= _userManager.GetCurrentUserId();
            
            dto.RepairDate =dto.RepairDatestr.PersianNumberToEnglish().ToMiladiDate(false);
            var equipmentFaulty = _mapper.Map<DomainClasses.Entity.EquipmentFaulty.EquipmentFaulty>(dto);
          
            equipmentFaulty.EquipmentFaultyFiles = new List<DomainClasses.Entity.EquipmentFaulty.EquipmentFaultyFile>();

            if (uploadByBase64.Name!= null )
            {
               

                    uploadByBase64.FolderType = UploadFolderType.FactorDoc;
                    var resultUpload = _uploadService.UploadImageAndPdfByBase64(uploadByBase64);
                    if (!string.IsNullOrWhiteSpace(resultUpload))
                    {
                        var fileType = (EquipmentFaultyFileType)int.Parse(uploadByBase64.CustomData);
                        
                        var transferFileFile = new DomainClasses.Entity.EquipmentFaulty.EquipmentFaultyFile()
                        {
                            Extension = Path.GetExtension(uploadByBase64.Name),
                            FileName = resultUpload,
                            FileType = fileType,
                            OriginalFileName = uploadByBase64.Name,
                            CreatedOn = DateTime.Now,

                        };

                        equipmentFaulty.EquipmentFaultyFiles.Add(transferFileFile);


                    }
                }
            


            _nezamEquipmentFaultys.Add(equipmentFaulty);

            var result = await SaveDbResult(_unitOfWork, equipmentFaulty);

            return result;
        }

        public async Task<DbResult> UpdateAsync(EquipmentFaultyDto dto)
        {

            var applicant = await _nezamEquipmentFaultys.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (applicant == null)
                return new DbResult(DbResult.M.NotFound);





            applicant.UserId = dto.UserId;
            applicant.RepairDate = dto.RepairDate;
            applicant.MoneyRepair = dto.MoneyRepair;
          
            applicant.Description = dto.Description;

            var result = await SaveDbResult(_unitOfWork, applicant);
            return result;

        }

        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var applicant = await _nezamEquipmentFaultys.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (applicant == null)
                return new DbResult(DbResult.M.NotFound);

            _unitOfWork.MarkAsSafeDelete(applicant);



            return await SaveDbResult(_unitOfWork);
        }

        public async Task<EquipmentFaultyDto> GetAsync(Guid? id = null, string code = null)
        {
            //var e = _nezamEquipmentFaultys.AsNoTracking().AsQueryable();
            var e = Dto();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();
            
            return await e.Cacheable().FirstOrDefaultAsync();
          
        }

        public async Task<IEnumerable<EquipmentFaultyDto>> GetAllExcelAsync(int? skip = null,
          int? take = null, DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null)
        {
            var e = (
                     from EquipmentFaulty in _nezamEquipmentFaultys
                     join equipment in _equipments on EquipmentFaulty.EquipmentId equals equipment.Id into equipmentJoin
                     from employe in equipmentJoin.DefaultIfEmpty()
                     where EquipmentFaulty.IsDeleted == false
                     select new
                     {
                         EquipmentFaulty.RepairDate,
                         EquipmentFaulty.Description,
                         EquipmentFaulty.MoneyRepair,
                         EquipmentFaulty.EquipmentId,
                         EquipmentFaulty.CreatedOn,
                         EquipmentFaulty.UserId,
                        

                     }).OrderBy(x => x.CreatedOn).AsQueryable();

           
            if (dateBuyFrom != null)
                e = e.Where(x => x.RepairDate >= dateBuyFrom).AsQueryable();

            if (dateBuyTo != null)
                e = e.Where(x => x.RepairDate <= dateBuyTo).AsQueryable();
            
            if (skip != null && take != null)
                e = e.Skip((skip.Value - 1) * take.Value).AsQueryable();

            if (take != null)
                e = e.Take(take.Value).AsQueryable();



            return await e.Select(x => new EquipmentFaultyDto()
            {
                Description= x.Description,
                MoneyRepair = x.MoneyRepair,
                RepairDate = x.RepairDate,
                UserId = x.UserId,
                EquipmentId = x.EquipmentId,
                CreatedOn = x.CreatedOn,

            }).ToListAsync();
        }

        public async Task<int> GetAllExcelCountAsync( DateTime? dateBuyFrom = null, DateTime? dateBuyTo = null)
        {
          
            var e = (
                    from EquipmentFaulty in _nezamEquipmentFaultys
                    join equipment in _equipments on EquipmentFaulty.EquipmentId equals equipment.Id into employeJoin
                    from employe in employeJoin.DefaultIfEmpty()
                    where EquipmentFaulty.IsDeleted == false
                    select new
                    {
                        EquipmentFaulty.RepairDate,
                        EquipmentFaulty.MoneyRepair,
                        EquipmentFaulty.Description,
                        EquipmentFaulty.CreatedOn,
                        EquipmentFaulty.EquipmentId,
                        EquipmentFaulty.UserId,

                    }).OrderBy(x => x.CreatedOn).AsQueryable();
                                    
                if (dateBuyFrom != null)
                e = e.Where(x => x.RepairDate >= dateBuyFrom).AsQueryable();

            if (dateBuyTo != null)
                e = e.Where(x => x.RepairDate <= dateBuyTo).AsQueryable();

           
            return await e.Select(x => new EquipmentFaultyDto()
            {
                RepairDate = x.RepairDate,
                MoneyRepair = x.MoneyRepair,
                Description = x.Description,
                UserId = x.UserId,
                EquipmentId = x.EquipmentId,
                CreatedOn = x.CreatedOn,
              

            }).CountAsync();
        }
      
        public async Task<GetAllTupleResult<EquipmentFaultyDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string fullName = null, string code = null,
            IList<Guid> ids = null , DateTime? fromCreatedOn = null, DateTime? toCreatedOn = null,Guid?equipmentId = null)
        {
            //var e = _nezamEquipmentFaultys.AsNoTracking().AsQueryable();
            var e = Dto();

          
         
           if (equipmentId != null)
                e = e.Where(x => x.EquipmentId == equipmentId).AsQueryable();
            if (fromCreatedOn != null)
            {
                var start = new DateTime(fromCreatedOn.Value.Year, fromCreatedOn.Value.Month, fromCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.RepairDate >= start).AsQueryable();

            }

            if (toCreatedOn != null)
            {
                var end = new DateTime(toCreatedOn.Value.Year, toCreatedOn.Value.Month, toCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.RepairDate < end).AsQueryable();
            }
            if (ids != null && ids.Any())
                e = e.Where(x => ids.Any(c => c == x.Id)).AsQueryable();
          
            return await e.ToGetAllTupleResult<EquipmentFaultyDto, EquipmentFaultyDto>(getAllTupleDto);
         
        }

        private IQueryable<EquipmentFaultyDto> Dto()
        {
            return (from EquipmentFaulty in _nezamEquipmentFaultys
                    join equipment in _equipments on EquipmentFaulty.EquipmentId equals equipment.Id into ps
                    from employ in ps.DefaultIfEmpty()
                    select new EquipmentFaultyDto()
                    {
                        Id = EquipmentFaulty.Id,
                        RepairDate = EquipmentFaulty.RepairDate,
                        MoneyRepair = EquipmentFaulty.MoneyRepair,
                        UserId = EquipmentFaulty.UserId,
                        EquipmentId = EquipmentFaulty.EquipmentId, 
                        Description = EquipmentFaulty.Description,
                     
                        //DateBuystr = ShamsiDateTimeExtension.ShamsiDateTime(EquipmentFaulty.DateBuy.Value, false),
                    }).AsQueryable();

        }


    }
}
