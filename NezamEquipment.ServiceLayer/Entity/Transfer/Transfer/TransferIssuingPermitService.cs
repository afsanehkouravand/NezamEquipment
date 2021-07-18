using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Entity.NezamEmploye;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto;
using NezamEquipment.ServiceLayer.Entity.Transfer.TransferMessage;
using NezamEquipment.ServiceLayer._Identity.User;
using AutoMapper;
using EFSecondLevelCache;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.Transfer
{
    public class TransferIssuingPermitService : BaseSaveDbResult, ITransferIssuingPermitService
    {
        private readonly IDbSet<DomainClasses.Entity.Transfer.TransferIssuingPermit> _transferIssuingPermits;
        private readonly IDbSet<DomainClasses.Entity.Transfer.Transfer> _transfer;
        private readonly IDbSet<NezamEmploye> _NezamEmployes;
        private readonly ITransferMessageService _transferMessageService;
        private readonly IUserManager _userManager;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        public TransferIssuingPermitService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper, ITransferMessageService transferMessageService, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _transferMessageService = transferMessageService;
            _transferIssuingPermits= _unitOfWork.Set<DomainClasses.Entity.Transfer.TransferIssuingPermit>();
            _transfer = _unitOfWork.Set<DomainClasses.Entity.Transfer.Transfer>();
            _NezamEmployes = _unitOfWork.Set<NezamEmploye>();
            _userManager = userManager;
        }

        public async Task<DbResult> AddAsync(TransferIssuingPermitDto transferIssuingPermitDto)
        {
            var transfer = await _transfer.Where(x => x.Id == transferIssuingPermitDto.TransferId).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);
            
            var checkContractNumber = await GetAsync(licenseNumber: transferIssuingPermitDto.LicenseNumber);
            if (checkContractNumber != null)
                return new DbResult("امکان ثبت شماره مجوز تکراری وجود ندارد.");
          
            var transferIssuingPermit = _mapper.Map<DomainClasses.Entity.Transfer.TransferIssuingPermit>(transferIssuingPermitDto);
            transferIssuingPermit.CreatedOn = DateTime.Now;
            transferIssuingPermit.CreatedByUserId = _userManager.GetCurrentUserId();
            transferIssuingPermit.IsDueDate= DateTime.Now;
            transfer.VerificationStepNew= TransferStepNew.IssuingPermit;
            transfer.VerificationStepOriginalNew = TransferStepNew.IssuingPermit;
            transfer.TransferStatusNew = TransferStatusNew.IssuingPermit;
           
            _transferIssuingPermits.Add(transferIssuingPermit);

            var result = await _unitOfWork.SaveChangesAsync();

            return new DbResult(result != 0);
        }
    
       
        public async Task<DbResult> EditAsync(TransferIssuingPermitDto dto)
        {
            var transferIssuingPermit = await _transferIssuingPermits.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (transferIssuingPermit == null)
                return new DbResult(DbResult.M.NotFound);

            transferIssuingPermit.IsDueDate = DateTime.Now;
           
            transferIssuingPermit.LicenseNumber = dto.LicenseNumber;
           
            transferIssuingPermit.Description = dto.Description;
           
            _unitOfWork.MarkAsChanged(transferIssuingPermit);
            
            return await SaveDbResult(_unitOfWork, transferIssuingPermit);

        }

        public async Task<GetAllTupleResult<TransferIssuingPermitDto>> GetAllContractNumberAsync(GetAllTupleDto getAllTupleDto = null,
            string lisenceNumber = null
        )
        {
            var e = Dto();

            if (lisenceNumber != null)
                e = e.Where(x => x.LicenseNumber.Contains(lisenceNumber)).AsQueryable();

            return await e.ToGetAllTupleResult<TransferIssuingPermitDto, TransferIssuingPermitDto>(getAllTupleDto);
        }
        public async Task<GetAllTupleResult<TransferIssuingPermitDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null,
            Guid? id = null, IList<Guid> ids = null, DateTime? fromCreatedOn = null,
            DateTime? createdOn = null, DateTime? toCreatedOn = null)
        {
            var e = Dto();

            if (ids != null && ids.Any())
                e = e.Where(x => ids.Any(c => c == x.Id));


            //if (contractNumber != null)
            //    e = e.Where(x => x.ContractNumber == contractNumber).AsQueryable();


            if (createdOn != null)
            {
                var start = new DateTime(createdOn.Value.Year, createdOn.Value.Month, createdOn.Value.Day, 0, 0, 0);
                var end = start.AddDays(1);
                e = e.Where(x => x.CreatedOn >= start && x.CreatedOn < end).AsQueryable();
            }
            if (fromCreatedOn != null)
            {
                var start = new DateTime(fromCreatedOn.Value.Year, fromCreatedOn.Value.Month, fromCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn >= start).AsQueryable();

            }

            if (toCreatedOn != null)
            {
                var end = new DateTime(toCreatedOn.Value.Year, toCreatedOn.Value.Month, toCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn < end).AsQueryable();
            }


            return await e.ToGetAllTupleResult<TransferIssuingPermitDto, TransferIssuingPermitDto>(getAllTupleDto);
        }


        public async Task<IEnumerable<TransferIssuingPermitDto>> GetAllPermitReportAsync(bool isLast, int? skip = null,
            int? take = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            
            var e = (
                from permits in _transferIssuingPermits
                join transfer in _transfer on permits.TransferId equals transfer.Id

                join applicant in _NezamEmployes on transfer.NezamEmployeId equals applicant.Id
               
                     select new
                     {
                         permits.Transfer.ContractNumber,
                         permits.Transfer.NezamEmploye.Fullname,
                         permits.Transfer.NezamEmploye.NationalCode,
                         permits.LicenseNumber,
                         permits.IsDueDate,
                         permits.Description,
                         permits.TransferId,
                         permits.CreatedOn,
                         applicant.Address,
                         applicant.FatherName,
                     
                     }).OrderBy(x => x.CreatedOn).AsQueryable();

            

            if (dateFrom != null)
                e = e.Where(x => x.CreatedOn >= dateFrom).AsQueryable();

            if (dateTo != null)
                e = e.Where(x => x.CreatedOn <= dateTo).AsQueryable();

            

            if (skip != null && take != null)
                e = e.Skip((skip.Value - 1) * take.Value).AsQueryable();

            if (take != null)
                e = e.Take(take.Value).AsQueryable();



            return await e.Select(x => new TransferIssuingPermitDto()
            {
                ContractNumber = x.ContractNumber,
                FullName = x.Fullname,
                NationalCode = x.NationalCode,
                IsDueDate = x.IsDueDate,
                Description = x.Description,
                LicenseNumber = x.LicenseNumber,
                TransferId = x.TransferId,
                FatherName = x.FatherName,
                Address = x.Address,


                CreatedOn = x.CreatedOn,

            }).ToListAsync();
        }
        public async Task<int> GetAllPermitReportCountAsync(bool hasCotazh, DateTime? dateFrom = null, DateTime? dateTo = null,
          string nameCompany = null)
        {

            var e = (
                from permits in _transferIssuingPermits
                join transfer in _transfer on permits.TransferId equals transfer.Id into transferJoin
                from transfer in transferJoin.DefaultIfEmpty()
                where permits.IsDeleted == false
                select new
                {
                    permits.IsDueDate,
                    permits.LicenseNumber,
                    permits.Description,
                    permits.Transfer.NezamEmploye.Fullname,
                    permits.Transfer.NezamEmploye.NationalCode,
                    permits.Transfer.ContractNumber,
                    permits.TransferId,
                    permits.CreatedOn,

                }).OrderBy(x => x.CreatedOn).AsQueryable();


           

            if (dateFrom != null)
                e = e.Where(x => x.CreatedOn >= dateFrom).AsQueryable();

            if (dateTo != null)
                e = e.Where(x => x.CreatedOn <= dateTo).AsQueryable();

           

            return await e.Select(x => new TransferIssuingPermitDto()
            {
                FullName = x.Fullname,
                NationalCode = x.NationalCode,
                IsDueDate = x.IsDueDate,
                Description = x.Description,
                LicenseNumber = x.LicenseNumber,
                TransferId = x.TransferId,
                ContractNumber = x.ContractNumber,

                CreatedOn = x.CreatedOn,
            }).CountAsync();
        }
        public async Task<TransferIssuingPermitDto> GetAsync(Guid? id = null, Guid? transferId = null, IList<Guid> ids = null, DateTime? fromCreatedOn = null, DateTime? createdOn = null,
            DateTime? toCreatedOn = null, string licenseNumber = null)
        {
            var e = Dto();
            if (id != null)
            {
                e = e.Where(x => x.Id == id).AsQueryable();
            }
          
            if (licenseNumber != null)
            {
                e = e.Where(x => x.LicenseNumber == licenseNumber).AsQueryable();
            }
            if (transferId != null)
                e = e.Where(x => x.TransferId == transferId).AsQueryable();

            if (createdOn != null)
            {
                var start = new DateTime(createdOn.Value.Year, createdOn.Value.Month, createdOn.Value.Day, 0, 0, 0);
                var end = start.AddDays(1);
                e = e.Where(x => x.CreatedOn >= start && x.CreatedOn < end).AsQueryable();
            }
            if (fromCreatedOn != null)
            {
                var start = new DateTime(fromCreatedOn.Value.Year, fromCreatedOn.Value.Month, fromCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn >= start).AsQueryable();

            }

            if (toCreatedOn != null)
            {
                var end = new DateTime(toCreatedOn.Value.Year, toCreatedOn.Value.Month, toCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn < end).AsQueryable();
            }


            return await e.Cacheable().FirstOrDefaultAsync();
        }

        private IQueryable<TransferIssuingPermitDto> Dto()
        {
            return (from transferIssuingPermit in _transferIssuingPermits
                    join transfer in _transfer on transferIssuingPermit.TransferId equals transfer.Id 

                    join applicant in _NezamEmployes on transfer.NezamEmployeId equals applicant.Id

                    select new TransferIssuingPermitDto()
                    {
                        LicenseNumber = transferIssuingPermit.LicenseNumber,                   
                        CreatedOn = transferIssuingPermit.CreatedOn,
                        IsDueDate = transferIssuingPermit.IsDueDate,
                        TransferId = transferIssuingPermit.TransferId,
                        Id = transferIssuingPermit.Id,
                        Description= transferIssuingPermit.Description,
                        FullName = transfer.NezamEmploye.Fullname,
                        NationalCode=transfer.NezamEmploye.NationalCode ,
                        ContractNumber = transfer.ContractNumber,
                        Address = applicant.Address,
                        FatherName =applicant.FatherName ,

                    }).AsNoTracking()
                .AsQueryable();
        }

        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var transfer = await _transferIssuingPermits.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);

            _unitOfWork.MarkAsSafeDelete(transfer);

            return await SaveDbResult(_unitOfWork);
        }

        //private async Task<int> GetTrackingCode()
        //{
        //    var generate = new Random();
        //    var code = generate.Next(1111111, 9999999);

        //    var result = await _Transfer.AsNoTracking().AnyAsync(x => x.TrackingCode == code);
        //    if (result)
        //        await GetTrackingCode();

        //    return code;
        //}

    }
}
