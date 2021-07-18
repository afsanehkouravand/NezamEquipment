using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.Normalization;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Entity.NezamEmploye;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Other.SmsLog;
using NezamEquipment.ServiceLayer.Entity.Transfer.ApplicantTransfer.Dto;
using NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto;
using NezamEquipment.ServiceLayer.Entity.Transfer.TransferMessage;
using NezamEquipment.ServiceLayer.OtherServices.Upload;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Enum;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer.OtherServices.UtilityService;
using AutoMapper;
using EFSecondLevelCache;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.Transfer
{
    public class TransferService : BaseSaveDbResult, ITransferService
    {
        private readonly IDbSet<DomainClasses.Entity.Transfer.Transfer> _transfer;
        private readonly IDbSet<DomainClasses.Entity.Transfer.PercentTransfer> _percentTransfers;
        private readonly IDbSet<NezamEmploye> _NezamEmployes;
        private readonly ITransferMessageService _transferMessageService;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IDbSet<DomainClasses.Entity.NezamEmploye.NezamEmploye> _applicantTransfers;
        private readonly ISmsLogService _smsLogService;
        private readonly IUserManager _userManager;
        private readonly IUtilityService _utilityService;

        public TransferService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper, ITransferMessageService transferMessageService, IUploadService uploadService,
            ISmsLogService smsLogService, IUserManager userManager, IUtilityService utilityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _transferMessageService = transferMessageService;
            _transfer = _unitOfWork.Set<DomainClasses.Entity.Transfer.Transfer>();
            _percentTransfers = _unitOfWork.Set<DomainClasses.Entity.Transfer.PercentTransfer>();
            _NezamEmployes = _unitOfWork.Set<NezamEmploye>();
            _uploadService = uploadService;
            _applicantTransfers = _unitOfWork.Set<DomainClasses.Entity.NezamEmploye.NezamEmploye>();
            _smsLogService = smsLogService;
            _userManager = userManager;
            _utilityService = utilityService;

        }

        public async Task<DbResult> AddAReportFilesync(Guid transferId, UploadByBase64Dto uploadByBase64Dto)
        {
            var transfer = await _transfer.Where(x => x.Id == transferId).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult("این پرونده وجود ندارد.");


            transfer.TransferReportFiles = new List<DomainClasses.Entity.Transfer.TransferReportFile>();
            if (uploadByBase64Dto.Name != null)
            {

                uploadByBase64Dto.FolderType = UploadFolderType.TransferReportFile;
                var resultUpload = _uploadService.UploadImageAndPdfByBase64(uploadByBase64Dto);
                if (!string.IsNullOrWhiteSpace(resultUpload))
                {
                    //var fileType = (TransferFileType)int.Parse(uploadByBase64Dto.CustomData);

                    var transferFileFile = new DomainClasses.Entity.Transfer.TransferReportFile()
                    {
                        Extension = Path.GetExtension(uploadByBase64Dto.Name),
                        FileName = resultUpload,
                        //FileType = fileType,
                        OriginalFileName = uploadByBase64Dto.Name,
                        CreatedOn = DateTime.Now,

                    };
                    transfer.TransferReportFiles.Add(transferFileFile);
                }
            }

            return await SaveDbResult(_unitOfWork);
        }

        public async Task<DbResult> AddAsync(TransferDto transferDto, List<ApplicantTransferDto> applicantTransferDtos, IList<UploadByBase64Dto> uploadByBase64Dtos)
        {
            var checkContractNumber = await GetAsync(contractNumber: transferDto.ContractNumber);
            if (checkContractNumber != null)
                return new DbResult("امکان ثبت شماره قرار داد تکراری وجود ندارد.");
            if (applicantTransferDtos != null)
            {
                foreach (var item in applicantTransferDtos)
                {
                    item.BirthDate = item.BirthDateStr.PersianNumberToEnglish().ToMiladiDate(false);
                    //if (
                    //    !_utilityService.IsValidNationalCode(item.NationalCode))
                    //{
                    //    return new DbResult("کد ملی وارد شده صحیح نمی باشد.");
                    //}

                }

            }

            var transfer = _mapper.Map<DomainClasses.Entity.Transfer.Transfer>(transferDto);
            transfer.CreatedOn = DateTime.Now;

            transfer.TransferStatus = TransferStatus.Waiting;
            transfer.TransferStatusNew = TransferStatusNew.Waiting;
            transfer.VerificationStep = TransferStep.InitialRegistration;
            transfer.VerificationStepNew = TransferStepNew.Completing;
            transfer.VerificationStepOriginal = TransferStep.MatchingDocuments;

            _transfer.Add(transfer);

            transfer.TransferFiles = new List<DomainClasses.Entity.Transfer.TransferFile>();

            if (uploadByBase64Dtos != null && uploadByBase64Dtos.Any())
            {
                foreach (var uploadByBase64Dto in uploadByBase64Dtos)
                {
                    if (string.IsNullOrWhiteSpace(uploadByBase64Dto.Name))
                        continue;

                    uploadByBase64Dto.FolderType = UploadFolderType.TransferFile;
                    var resultUpload = _uploadService.UploadImageAndPdfByBase64(uploadByBase64Dto);
                    if (!string.IsNullOrWhiteSpace(resultUpload))
                    {
                        var fileType = (TransferFileType)int.Parse(uploadByBase64Dto.CustomData);


                        var transferFileFile = new DomainClasses.Entity.Transfer.TransferFile()
                        {
                            Extension = Path.GetExtension(uploadByBase64Dto.Name),
                            FileName = resultUpload,
                            //TransferId = transfer.Id,
                            FileType = fileType,
                            OriginalFileName = uploadByBase64Dto.Name,
                            CreatedOn = DateTime.Now,

                        };
                        transfer.TransferFiles.Add(transferFileFile);


                    }
                }
            }

            if (applicantTransferDtos != null)
            {
                transfer.ApplicantTransfers = new List<DomainClasses.Entity.Transfer.ApplicantTransfer>();
                foreach (var item in applicantTransferDtos)
                {

                    var applicantTransfer = _mapper.Map<DomainClasses.Entity.Transfer.ApplicantTransfer>(item);

                    applicantTransfer.CreatedOn = DateTime.Now;

                    transfer.ApplicantTransfers.Add(applicantTransfer);

                }

            }

            var result = await SaveDbResult(_unitOfWork, transfer);

            return result;
        }

        public async Task<DbResult> ReSendAsync(Guid id, string message)
        {
            var transfer = await _transfer.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);

            transfer.IsSuspensionReSended = true;
            transfer.IsSuspension = false;
            transfer.SuspensionStep = null;


            if (!string.IsNullOrEmpty(message))
            {
                var transferMessage = _transferMessageService.New(message: message);
                transferMessage.Verification = true;
                transferMessage.MessageType = MessageType.Merchent;
                transferMessage.CreatedOn = DateTime.Now;

                transfer.TransferMessages =
                    new List<DomainClasses.Entity.Transfer.TransferMessage>()
                    {
                        transferMessage
                    };
            }

            _unitOfWork.MarkAsChanged(transfer);


            return await SaveDbResult(_unitOfWork, transfer);
        }

        public async Task<DbResult> SendAsync(Guid id, string address = null)
        {
            var transfer = await _transfer.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);
            if (address != null)
                transfer.Address = address;
            if (transfer.IsSuspension)
            {
                transfer.IsSuspension = false;
                transfer.SuspensionStep = null;
                transfer.TransferStatusNew = TransferStatusNew.Waiting;
                transfer.VerificationStepNew = TransferStepNew.Completing;
                transfer.VerificationStepOriginal = TransferStep.MatchingDocuments;
            }
            else
            {
                transfer.TransferStatus = TransferStatus.Waiting;
                transfer.TransferStatusNew = TransferStatusNew.Waiting;
                transfer.VerificationStep = TransferStep.InitialRegistration;
                transfer.VerificationStepOriginal = TransferStep.MatchingDocuments;

            }

            _unitOfWork.MarkAsChanged(transfer);


            return await SaveDbResult(_unitOfWork, transfer);
        }

        public async Task<DbResult> EditAsync(TransferDto dto)
        {
            var transfer = await _transfer.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);
            //var checkContractNumber = await GetAsync(contractNumber: dto.ContractNumber);
            //if (checkContractNumber != null)
            //    return new DbResult("امکان ثبت شماره قرار داد تکراری وجود ندارد.");

            //transfer.ContractNumber = dto.ContractNumber;
            //transfer.ContractDate = dto.ContractDate;

            //transfer.OthersContractParty = dto.OthersContractParty;
            //transfer.FatherName = dto.FatherName;
            //transfer.OthersContractParty = dto.OthersContractParty;
            //transfer.NationalCode = dto.NationalCode;
            //transfer.ExpiredExpirationDate = dto.ExpiredExpirationDate;
            //transfer.TitleOfActivity = dto.TitleOfActivity;
            transfer.Address = dto.Address;
            transfer.RegisterPlak = dto.RegisterPlak;
            transfer.Bakhsh = dto.Bakhsh;
            transfer.PostalCode = dto.PostalCode;
            transfer.Area = dto.Area;
            transfer.UserType = dto.UserType;

            //transfer.Description = dto.Description;
            //transfer.TransferStatus = dto.TransferStatus;
            //transfer.VerificationStep = dto.VerificationStep;
            //transfer.VerificationStepOriginal = dto.VerificationStepOriginal;
            //transfer.IsSuspension = dto.IsSuspension;
            //transfer.SuspensionStep = dto.SuspensionStep;
            //transfer.IsSuspensionReSended = dto.IsSuspensionReSended;
            _unitOfWork.MarkAsChanged(transfer);

            return await SaveDbResult(_unitOfWork, transfer);

        }
        public async Task<DbResult> UpdateAsync(Guid id, TransferStatusNew transferStatus)
        {
            var transfer = await _transfer.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);

            transfer.TransferStatusNew = (TransferStatusNew)transferStatus;

            _unitOfWork.MarkAsChanged(transfer);

            var result = await SaveDbResult(_unitOfWork, transfer);


            return result;
        }
        public async Task<DbResult> UpdateFinancialAsync(TransferDto dto)
        {
            var transfer = await _transfer.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);

            transfer.IsFinancial = dto.IsFinancial1;
            transfer.FinancialDate = DateTime.Now;
            transfer.DescriptionFinancial = dto.DescriptionFinancial1;
            transfer.TransferStatusNew = TransferStatusNew.Financial;
            transfer.VerificationStepNew = TransferStepNew.PayAvarez;

            _unitOfWork.MarkAsChanged(transfer);

            var result = await SaveDbResult(_unitOfWork, transfer);


            return result;
        }
        public async Task<DbResult> UpdateAsync(Guid id, TransferStepNew step, string message, TransferStepNew stepOrginal,
            bool suspension, bool verification, TransferStepNew? returnStep = null, TransferStepNew? gotoStep = null,
            bool? sendSms = null, bool? addMessage = null, TransferStepNew? suspensionStep = null, TransferStatusNew? transferStatus = null)
        {
            var transfer = await _transfer.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);
            var applicant = _applicantTransfers.FirstOrDefault(x => x.Id == transfer.NezamEmployeId);

            //var historyChangeOldData = _mapper.Map<TransferDto>(Transfer);
            var user = await _userManager.GetAsync(_userManager.GetCurrentUserId());

            var transferMessage = _transferMessageService.New(message: message);
            transferMessage.StepNew = step;

            //var description = string.Empty;

            // رد
            if (suspension && !verification)
            {

                transfer.IsSuspension = true;
                transfer.SuspensionStepNew = suspensionStep ?? step;
                transfer.IsSuspensionReSended = false;
                transfer.VerificationStepNew = step;
                transfer.VerificationStepOriginalNew = stepOrginal;
                transferMessage.Suspension = true;
                transferMessage.CreatedByUserId = user.Id;


                await _smsLogService.SendRedirectTransferAsync(applicant.MobileNumber, transfer.ContractNumber);

                //description =
                //$"خروج از صف توسط کارشناس {step.GetDisplayName()} {user.FirstName} {user.LastName}، با پاسخ: \r\n {message}";
            }
            // تایید
            if (!suspension && verification)
            {
                long due = (long)transfer.DuesTransfer;

                transfer.VerificationStepNew = step;
                transfer.VerificationStepOriginalNew = stepOrginal;
                transfer.IsSuspension = false;
                transfer.SuspensionStepNew = null;
                transferMessage.Verification = true;
                await _smsLogService.SendDuesTransferAsync(applicant.MobileNumber, due.ToString(),
                    transfer.ContractNumber);

                //description =
                //    $"تایید توسط کارشناس {step.GetDisplayName()} {user.FirstName} {user.LastName}، با پاسخ: \r\n {message}";
            }
            if (transferStatus != null)
            {
                transfer.TransferStatusNew = (TransferStatusNew)transferStatus;
                transfer.VerificationStepNew = stepOrginal;
                if (transfer.TransferStatusNew == TransferStatusNew.Confirmed)
                {
                    var licenseValidity = new DomainClasses.Entity.Transfer.LicenseValidityTransfer()
                    {
                        TransferId = transfer.Id,
                        ContractDate = DateTime.Now,
                        ExpiredExpirationDate = DateTime.Now.AddMonths(3),
                        UserId = _userManager.GetCurrentUserId(),
                        CreatedOn = DateTime.Now,

                    };
                    transfer.LicenseValidityTransfers.Add(licenseValidity);

                    await _smsLogService.SendConfirmedTransferAsync(applicant.MobileNumber,
                  transfer.ContractNumber);
                }
            }

            // بازگشت
            if (!suspension && !verification && returnStep != null)
            {
                transfer.VerificationStepNew = returnStep.Value;
                transfer.VerificationStepOriginalNew = stepOrginal;

                //description =
                //    $"بازگشت پرونده توسط کارشناس {step.GetDisplayName()} {user.FirstName} {user.LastName} به کارشناس {returnStep.GetDisplayName()}، با پاسخ: \r\n {message}";
            }

            // ارسال
            if (!suspension && !verification && gotoStep != null)
            {
                transfer.VerificationStepNew = gotoStep.Value;
                transfer.VerificationStepOriginalNew = stepOrginal;


                //description =
                //    $"ارسال پرونده توسط کارشناس {step.GetDisplayName()} {user.FirstName} {user.LastName} به کارشناس {gotoStep.GetDisplayName()}";
            }

            // اگر نال بود بفرسته - اکر نال نبود ولی فالز بود نفرسته
            var doAddMessage = !(addMessage != null && addMessage.Value == false);
            if (doAddMessage)
            {
                transfer.TransferMessages = new List<DomainClasses.Entity.Transfer.TransferMessage>()
                {
                    transferMessage
                };
            }

            _unitOfWork.MarkAsChanged(transfer);

            var result = await SaveDbResult(_unitOfWork, transfer);


            return result;
        }
        public async Task<DbResult> UpdateArenaAndLandAsync(TransferDto transferDto)
        {
            var transfer = await _transfer.Where(x => x.Id == transferDto.Id).FirstOrDefaultAsync();


            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);
            transfer.Description = transferDto.Description;
            transfer.LandEarth = transferDto.LandEarth;
            transfer.ArenaEarth = transferDto.ArenaEarth;
            transfer.LandArenaArea = transferDto.LandArenaArea;
            transfer.LandArea = transferDto.LandArea;
            var arse = transferDto.ArenaEarth * transferDto.LandArenaArea;
            var ayan = transferDto.LandEarth * transferDto.LandArea;
            transfer.ArenaAndLand = arse + ayan;
            //transfer.FivePercentArenaAndLand = (double) ((transfer.ArenaAndLand * transferDto.FivePercent) / 100);
            //var tow = (transfer.ArenaAndLand * transferDto.LandArea);
            //transfer.TowPercentArenaAndLand = (double) ((tow * transferDto.TowPercent)/100);

            transfer.TotalValue = transferDto.CommercialValue + arse + ayan;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (transferDto.Ownership == Ownership.Special && (transferDto.SpecialOwnership != 0))
            {
                var b = transferDto.SpecialOwnership / 6;
                transfer.DuesTransfer = ((0.05) * transfer.TotalValue) * b;

                transfer.SpecialOwnership = transferDto.SpecialOwnership;

            }
            else
            {
                int intownerShip = Convert.ToInt32((transferDto.Ownership)) + 1; ;
                var a = (double)intownerShip / 6;
                transfer.DuesTransfer = ((0.05) * transfer.TotalValue) * a;
                if (transfer.SpecialOwnership != 0)
                {
                    transfer.SpecialOwnership = 0;
                }


            }
            transfer.Ownership = transferDto.Ownership;
            transfer.CommercialValue = transferDto.CommercialValue;
            transfer.TransferStatusNew = TransferStatusNew.ManagementCooperative;
            transfer.VerificationStepNew = TransferStepNew.Review;

            //transfer.DuesTransfer =( (transfer.TowPercentArenaAndLand + transfer.FivePercentArenaAndLand)*.7142);
            //transfer.TotalValue = (transfer.DuesTransfer * 100)/5;
            //transfer.CommercialValue = transfer.TotalValue - arse - ayan;
            _unitOfWork.MarkAsChanged(transfer);

            var result = await SaveDbResult(_unitOfWork, transfer);


            return result;
        }

        public async Task<GetAllTupleResult<TransferDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null, TransferStepNew? step = null, TransferStatusNew? status = null,
            Guid? id = null, IList<Guid> ids = null, Guid? applicantId = null, DateTime? fromCreatedOn = null, bool? isSuspension = null,
            DateTime? createdOn = null, DateTime? toCreatedOn = null, TransferStatusNew? notstep = null,
            bool? isSuspensionStep = null, string fromCreatedOnstr = null, string toCreatedOnstr = null)
        {
            var e = Dto();

            if (ids != null && ids.Any())
                e = e.Where(x => ids.Any(c => c == x.Id));

            if (applicantId != null)
                e = e.Where(x => x.NezamEmployeId == applicantId).AsQueryable();


            if (status != null)
            {
                if (status == TransferStatusNew.ManagementTow)
                {
                    e = e.Where(x => x.TransferStatusNew == TransferStatusNew.ManagementTow || x.TransferStatusNew == TransferStatusNew.ManagementOneConfirmed ||
                                     x.TransferStatusNew == TransferStatusNew.Financial || x.TransferStatusNew == TransferStatusNew.Confirmed).AsQueryable();
                }
                else if (status == TransferStatusNew.Payment)
                {
                    e = e.Where(x => x.TransferStatusNew == TransferStatusNew.Payment || x.TransferStatusNew == TransferStatusNew.Debtor).AsQueryable();

                }
                else if (status == TransferStatusNew.ManagementOne)
                {
                    e = e.Where(x => x.TransferStatusNew == TransferStatusNew.ManagementOne || x.TransferStatusNew == TransferStatusNew.Financial
                                                                                            || x.TransferStatusNew == TransferStatusNew.Confirmed).AsQueryable();

                }
                else
                {
                    e = e.Where(x => x.TransferStatusNew == status).AsQueryable();
                }


            }


            if (notstep != null)
                e = e.Where(x => x.TransferStatusNew != notstep).AsQueryable();
            if (status != null && status == TransferStatusNew.None)
            {
                e = e.Where(x => x.IsSuspension == true).AsQueryable();
            }

            if (isSuspension != null)
                e = e.Where(x => x.IsSuspension == isSuspension).AsQueryable();

            if (isSuspensionStep != null)
                e = isSuspensionStep.Value
                    ? e.Where(x => x.SuspensionStep != null).AsQueryable()
                    : e.Where(x => x.SuspensionStep == null).AsQueryable();


            if (step != null)
                e = e.Where(x => x.VerificationStepNew == step).AsQueryable();

            if (contractNumber != null)
                e = e.Where(x => x.ContractNumber == contractNumber).AsQueryable();


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
            if (fromCreatedOnstr != null)
            {
                var datefromCreatedOn = fromCreatedOnstr.PersianNumberToEnglish().ToMiladiDate(false);
                var start = new DateTime(datefromCreatedOn.Year, datefromCreatedOn.Month, datefromCreatedOn.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn >= start).AsQueryable();

            }
            if (toCreatedOnstr != null)
            {
                var datetoCreatedOnstr = toCreatedOnstr.PersianNumberToEnglish().ToMiladiDate(false);
                var end = new DateTime(datetoCreatedOnstr.Year, datetoCreatedOnstr.Month, datetoCreatedOnstr.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn < end).AsQueryable();
            }
            return await e.ToGetAllTupleResult<TransferDto, TransferDto>(getAllTupleDto);
        }
        public async Task<GetAllTupleResult<TransferDto>> GetAllExtendedLicenseAsync(GetAllTupleDto getAllTupleDto = null,
           string contractNumber = null, string registerPlak = null, TransferUserType? transferUserType = null,
           Guid? id = null, IList<Guid> ids = null, DateTime? fromCreatedOn = null,
           DateTime? createdOn = null, DateTime? toCreatedOn = null,
           string fromCreatedOnstr = null, string toCreatedOnstr = null)
        {
            var e = Dto();

            if (ids != null && ids.Any())
                e = e.Where(x => ids.Any(c => c == x.Id));

            if (registerPlak != null)
            {

                e = e.Where(x => x.RegisterPlak == registerPlak).AsQueryable();

            }


            if (transferUserType != null)
                e = e.Where(x => x.UserType == transferUserType).AsQueryable();


            if (contractNumber != null)
                e = e.Where(x => x.ContractNumber == contractNumber).AsQueryable();


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
            if (fromCreatedOnstr != null)
            {
                var datefromCreatedOn = fromCreatedOnstr.PersianNumberToEnglish().ToMiladiDate(false);
                var start = new DateTime(datefromCreatedOn.Year, datefromCreatedOn.Month, datefromCreatedOn.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn >= start).AsQueryable();

            }
            if (toCreatedOnstr != null)
            {
                var datetoCreatedOnstr = toCreatedOnstr.PersianNumberToEnglish().ToMiladiDate(false);
                var end = new DateTime(datetoCreatedOnstr.Year, datetoCreatedOnstr.Month, datetoCreatedOnstr.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn < end).AsQueryable();
            }
            return await e.ToGetAllTupleResult<TransferDto, TransferDto>(getAllTupleDto);
        }

        public async Task<GetAllTupleResult<TransferDto>> GetAllContractNumberAsync(GetAllTupleDto getAllTupleDto = null,
          string contractNumber = null
         )
        {
            var e = Dto();

            if (contractNumber != null)
                e = e.Where(x => x.ContractNumber.Contains(contractNumber)).AsQueryable();

            return await e.ToGetAllTupleResult<TransferDto, TransferDto>(getAllTupleDto);
        }

        public async Task<GetAllTupleResult<TransferDto>> GetAllResendAsync(GetAllTupleDto getAllTupleDto = null,
            string contractNumber = null,
            Guid? id = null, IList<Guid> ids = null, Guid? applicantId = null, DateTime? fromCreatedOn = null,
            DateTime? createdOn = null, DateTime? toCreatedOn = null)
        {
            var e = Dto();
            e = e.Where(x => x.IsSuspension == true);

            if (ids != null && ids.Any())
                e = e.Where(x => ids.Any(c => c == x.Id));

            if (applicantId != null)
                e = e.Where(x => x.NezamEmployeId == applicantId).AsQueryable();

            if (contractNumber != null)
                e = e.Where(x => x.ContractNumber == contractNumber).AsQueryable();


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


            return await e.ToGetAllTupleResult<TransferDto, TransferDto>(getAllTupleDto);
        }

        public async Task<TransferDto> GetAsync(Guid? id = null, IList<Guid> ids = null, Guid? applicantId = null, DateTime? fromCreatedOn = null, DateTime? createdOn = null,
            DateTime? toCreatedOn = null, string contractNumber = null)
        {
            var e = Dto();
            if (id != null)
            {
                e = e.Where(x => x.Id == id).AsQueryable();
            }
            if (contractNumber != null)
            {
                e = e.Where(x => x.ContractNumber == contractNumber).AsQueryable();
            }
            if (applicantId != null)
                e = e.Where(x => x.NezamEmployeId == applicantId).AsQueryable();

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


        private IQueryable<TransferDto> Dto()
        {
            return (from transfer in _transfer
                    join applicant in _NezamEmployes on transfer.NezamEmployeId equals applicant.Id

                    select new TransferDto()
                    {
                        RegisterPlak = transfer.RegisterPlak,
                        Bakhsh = transfer.Bakhsh,
                        PostalCode = transfer.PostalCode,
                        Area = transfer.Area,
                        UserType = transfer.UserType,
                        ContractNumber = transfer.ContractNumber,
                        ContractDate = transfer.ContractDate,
                        OthersContractParty = transfer.OthersContractParty,
                        NationalCode = applicant.NationalCode,
                        Fullname = applicant.Fullname,
                        ExpiredExpirationDate = transfer.ExpiredExpirationDate,
                        TitleOfActivity = transfer.TitleOfActivity,
                        Address = transfer.Address,
                        Description = transfer.Description,
                        TransferStatus = transfer.TransferStatus,
                        FatherName = transfer.FatherName,
                        CreatedOn = transfer.CreatedOn,
                        NezamEmployeId = transfer.NezamEmployeId,
                        SuspensionStep = transfer.SuspensionStep,
                        Id = transfer.Id,
                        VerificationStepOriginal = transfer.VerificationStepOriginal,
                        VerificationStep = transfer.VerificationStep,
                        IsSuspension = transfer.IsSuspension,
                        IsSuspensionReSended = transfer.IsSuspensionReSended,
                        LandArea = transfer.LandArea,
                        ArenaEarth = transfer.ArenaEarth,
                        LandEarth = transfer.LandEarth,
                        TowPercentArenaAndLand = transfer.TowPercentArenaAndLand,
                        FivePercentArenaAndLand = transfer.FivePercentArenaAndLand,
                        ArenaAndLand = transfer.ArenaAndLand,
                        DuesTransfer = transfer.DuesTransfer,
                        TotalValue = transfer.TotalValue,
                        CommercialValue = transfer.CommercialValue,
                        LandArenaArea = transfer.LandArenaArea,
                        Ownership = transfer.Ownership,
                        SpecialOwnership = transfer.SpecialOwnership,
                        IsFinancial1 = transfer.IsFinancial,
                        TransferStatusNew = transfer.TransferStatusNew,
                        VerificationStepNew = transfer.VerificationStepNew,

                    }).AsQueryable();

        }


        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var transfer = await _transfer.Where(x => x.Id == id).FirstOrDefaultAsync();
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
