using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Transfer.Transfer.Dto;
using NezamEquipment.ServiceLayer.Entity.Transfer.TransferFile.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Enum;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.TransferFile
{
    public class TransferFileService : BaseSaveDbResult, ITransferFileService
    {
        private readonly IDbSet<DomainClasses.Entity.Transfer.TransferFile> _transferFiles;
        private readonly IDbSet<DomainClasses.Entity.Transfer.Transfer> _transfers;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
     
        public TransferFileService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _transferFiles = _unitOfWork.Set<DomainClasses.Entity.Transfer.TransferFile>();
            _transfers = _unitOfWork.Set<DomainClasses.Entity.Transfer.Transfer>();

        }

        public async Task<DbResult> AddAsync(Guid applicantId, Guid transferId, IList<UploadByBase64Dto> uploadByBase64Dtos)
        {
            var transfer = await _transfers.Where(x => x.NezamEmployeId == applicantId && x.Id == transferId).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);

            var files = await _transferFiles.Where(x => x.TransferId == transferId)
                .ToListAsync();

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

                        var file = files.FirstOrDefault(x => x.FileType == fileType);
                        if (file != null)
                        {
                            _unitOfWork.MarkAsSafeDelete(file);
                        }

                        var transferFileFile = new DomainClasses.Entity.Transfer.TransferFile()
                        {
                            Extension = Path.GetExtension(uploadByBase64Dto.Name),
                            FileName = resultUpload,
                            TransferId = transferId,
                            FileType = fileType,
                            OriginalFileName = uploadByBase64Dto.Name,
                            CreatedOn = DateTime.Now,
     
                        };

                        _transferFiles.Add(transferFileFile);
                     
                    }
                }
            }

            return await SaveDbResult(_unitOfWork, transfer);
        }
        public async Task<DbResult> UpdateAsync(Guid applicantId, Guid id, IList<UploadByBase64Dto> uploadByBase64Dtos)
        {
            var transfer = await _transfers.Where(x => x.NezamEmployeId == applicantId && x.Id == id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);

            var files = await _transferFiles.Where(x => x.TransferId == id)
                .ToListAsync();

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

                        var file = files.FirstOrDefault(x => x.FileType == fileType);
                        if (file != null)
                        {
                            file.FileName = resultUpload;
                            file.Extension = Path.GetExtension(uploadByBase64Dto.Name);
                            file.OriginalFileName = uploadByBase64Dto.Name;
                            _unitOfWork.MarkAsChanged(file);
                        }
                        else
                        {

                            var awesomePemarkFile = new DomainClasses.Entity.Transfer.TransferFile()
                            {
                                Extension = Path.GetExtension(uploadByBase64Dto.Name),
                                FileName = resultUpload,
                                FileType = fileType,
                                OriginalFileName = uploadByBase64Dto.Name,
                                CreatedOn = DateTime.Now,
                                TransferId = transfer.Id,
                            };

                            _transferFiles.Add(awesomePemarkFile);
                        }


                    }
                }
            }

            return await SaveDbResult(_unitOfWork);
        }

        public async Task<DbResult> UpdateFileAsync(Guid id, TransferFileStatus status, UploadByBase64Dto uploadByBase64Dto )
        {
          

            var files = await _transferFiles.Where(x => x.Id == id)
                .ToListAsync();

           

                    uploadByBase64Dto.FolderType = UploadFolderType.TransferFile;
                    var resultUpload = _uploadService.UploadImageAndPdfByBase64(uploadByBase64Dto);
                    if (!string.IsNullOrWhiteSpace(resultUpload))
                    {
                        var fileType = (TransferFileType)int.Parse(uploadByBase64Dto.CustomData);

                        var file = files.FirstOrDefault(x => x.FileType == fileType);
                        if (file != null)
                        {
                            file.FileName = resultUpload;
                            file.Extension = Path.GetExtension(uploadByBase64Dto.Name);
                            file.OriginalFileName = uploadByBase64Dto.Name;
                       file.Status = status;
                            _unitOfWork.MarkAsChanged(file);
                        }
                        else
                        {

                            var awesomePemarkFile = new DomainClasses.Entity.Transfer.TransferFile()
                            {
                                Extension = Path.GetExtension(uploadByBase64Dto.Name),
                                FileName = resultUpload,
                                FileType = fileType,
                                OriginalFileName = uploadByBase64Dto.Name,
                                CreatedOn = DateTime.Now,
                                
                            };

                            _transferFiles.Add(awesomePemarkFile);
                        }

                      
                    }
            

            return await SaveDbResult(_unitOfWork);
        }
        public async Task<DbResult> UpdateAsync(Guid id, TransferFileStatus status)
        {
            var transferFile = await _transferFiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transferFile == null)
                return new DbResult(DbResult.M.NotFound);

            transferFile.Status = status;

            _unitOfWork.MarkAsChanged(transferFile);


            return await SaveDbResult(_unitOfWork, transferFile);
        }

        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var transferFileFile = await _transferFiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transferFileFile == null)
                return new DbResult(DbResult.M.NotFound);

            _unitOfWork.MarkAsSafeDelete(transferFileFile);


            var result = await SaveDbResult(_unitOfWork, transferFileFile);

            return result;
        }

   
        public async Task<GetAllTupleResult<TransferFileDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            Guid? transferFileId = null, TransferFileStatus? status=null,Guid? transferId=null)
        {
            var e = Dto();

            if (transferFileId != null)
                e = e.Where(x => x.Id == transferFileId).AsQueryable();

            if (transferId != null)
                e = e.Where(x => x.TransferId == transferId).AsQueryable();

            if (status != null)
                e = e.Where(x => x.Status == status).AsQueryable();

            return await e.ToGetAllTupleResult<TransferFileDto, TransferFileDto>(getAllTupleDto);
        }

       
        public async Task<IEnumerable<TransferFileDto>> GetFileAsync(
            Guid? transferFileId = null, TransferFileStatus? status = null, Guid? transferId = null)
        {
            var e = _transferFiles.AsNoTracking().AsQueryable();

            if (transferFileId != null)
                e = e.Where(x => x.Id == transferFileId).AsQueryable();

            if (transferId != null)
                e = e.Where(x => x.TransferId == transferId).AsQueryable();

            if (status != null)
                e = e.Where(x => x.Status == status).AsQueryable();

            return await e.ProjectTo<TransferFileDto>(_mapper.ConfigurationProvider).Cacheable().ToListAsync();
        }
        public async Task<TransferFileDto> GetAsync(Guid? id = null,Guid? transferId=null)
        {
            var e = Dto();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();
            if (transferId != null)
                e = e.Where(x => x.TransferId == transferId).AsQueryable();

            return await e.Cacheable().FirstOrDefaultAsync();

        }

        public List<TransferDto> GetListAsync(GetAllTupleDto getAllTupleDto = null, bool? all = null)
        {
            var e = DtoDocumnetRegister();
            var transfer = new List<TransferDto>();
            if (all == true)
            {
                foreach (var item in e)
                {

                    transfer.Add(item);
                }
                return transfer;
            }
            foreach (var item in e)
            {
                var isRow = _transferFiles.Where(x => x.TransferId == item.Id).ToList();
                if (!isRow.Any())
                {
                    transfer.Add(item);
                }

            }
            return transfer;

        }

        private IQueryable<TransferFileDto> Dto()
        {
            return (from applicantTransfer in _transferFiles
                    join transfer in _transfers on applicantTransfer.TransferId equals transfer.Id

                    select new TransferFileDto()
                    {
                       
                        FileName = applicantTransfer.FileName,
                        Extension = applicantTransfer.Extension,
                        OriginalFileName = applicantTransfer.OriginalFileName,
                        FileType = applicantTransfer.FileType,
                        Status = applicantTransfer.Status,
                        TransferId = transfer.Id,                   
                        CreatedOn = applicantTransfer.CreatedOn,                   
                        Id = applicantTransfer.Id,                     
                        TransferStatus = transfer.TransferStatus,
                        VerificationStep = transfer.VerificationStep,
                        IsSuspension = transfer.IsSuspension,
                        IsSuspensionReSended = transfer.IsSuspensionReSended
                    }).AsNoTracking()
                .AsQueryable();
        }


        private IQueryable<TransferDto> DtoDocumnetRegister()
        {

            return (from transfer in _transfers

                    select new TransferDto()
                    {
                        Id = transfer.Id,
                        ContractNumber = transfer.ContractNumber,
                    }).AsNoTracking()
                .AsQueryable();
        }
    }
}
