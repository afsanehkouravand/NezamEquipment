using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Dto;
using NezamEquipment.ServiceLayer.OtherServices.Upload.Enum;

namespace NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile
{
    public class EquipmentFaultyFileService : BaseSaveDbResult, IEquipmentFaultyFileService 
    {
        private readonly IDbSet<DomainClasses.Entity.EquipmentFaulty.EquipmentFaultyFile> _equipmentFaultyFiles;
        private readonly IDbSet<DomainClasses.Entity.EquipmentFaulty.EquipmentFaulty> _equipmentFaultys;
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
     
        public EquipmentFaultyFileService(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _equipmentFaultyFiles = _unitOfWork.Set<DomainClasses.Entity.EquipmentFaulty.EquipmentFaultyFile>();
            _equipmentFaultys = _unitOfWork.Set<DomainClasses.Entity.EquipmentFaulty.EquipmentFaulty>();

        }

        public async Task<DbResult> AddAsync(Guid applicantId, Guid EquipmentFaultyId, IList<UploadByBase64Dto> uploadByBase64Dtos)
        {
            //var EquipmentFaulty = await _EquipmentFaultys.Where(x => x.EquipmentId == applicantId && x.Id == EquipmentFaultyId).FirstOrDefaultAsync();
            //if (EquipmentFaulty == null)
            //    return new DbResult(DbResult.M.NotFound);

            var files = await _equipmentFaultyFiles.Where(x => x.EquipmentFaultyId == EquipmentFaultyId)
                .ToListAsync();

            if (uploadByBase64Dtos != null && uploadByBase64Dtos.Any())
            {
                foreach (var uploadByBase64Dto in uploadByBase64Dtos)
                {
                    if (string.IsNullOrWhiteSpace(uploadByBase64Dto.Name))
                        continue;

                    uploadByBase64Dto.FolderType = UploadFolderType.FactorDoc;
                    var resultUpload = _uploadService.UploadImageAndPdfByBase64(uploadByBase64Dto);
                    if (!string.IsNullOrWhiteSpace(resultUpload))
                    {
                       
                        var EquipmentFaultyFileFile = new DomainClasses.Entity.EquipmentFaulty.EquipmentFaultyFile()
                        {
                            Extension = Path.GetExtension(uploadByBase64Dto.Name),
                            FileName = resultUpload,
                            EquipmentFaultyId = EquipmentFaultyId,
                           
                            OriginalFileName = uploadByBase64Dto.Name,
                            CreatedOn = DateTime.Now,
     
                        };

                        _equipmentFaultyFiles.Add(EquipmentFaultyFileFile);
                     
                    }
                }
            }

            return await SaveDbResult(_unitOfWork);
        }
        public async Task<DbResult> UpdateAsync(Guid equipmentFaultyId, Guid id, IList<UploadByBase64Dto> uploadByBase64Dtos)
        {
            //var EquipmentFaulty = await _EquipmentFaultys.Where(x => x.ApplicantRentAndEquipmentFaultyId == applicantId && x.Id == id).FirstOrDefaultAsync();
            //if (EquipmentFaulty == null)
            //    return new DbResult(DbResult.M.NotFound);

            var files = await _equipmentFaultyFiles.Where(x => x.EquipmentFaultyId == id)
                .ToListAsync();

            if (uploadByBase64Dtos != null && uploadByBase64Dtos.Any())
            {
                foreach (var uploadByBase64Dto in uploadByBase64Dtos)
                {
                    if (string.IsNullOrWhiteSpace(uploadByBase64Dto.Name))
                        continue;

                    uploadByBase64Dto.FolderType = UploadFolderType.FactorDoc;
                    var resultUpload = _uploadService.UploadImageAndPdfByBase64(uploadByBase64Dto);
                    if (!string.IsNullOrWhiteSpace(resultUpload))
                    {
                        var fileType = (EquipmentFaultyFileType)int.Parse(uploadByBase64Dto.CustomData);

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

                            var awesomePemarkFile = new DomainClasses.Entity.EquipmentFaulty.EquipmentFaultyFile()
                            {
                                Extension = Path.GetExtension(uploadByBase64Dto.Name),
                                FileName = resultUpload,
                                FileType = fileType,
                                OriginalFileName = uploadByBase64Dto.Name,
                                CreatedOn = DateTime.Now,
                                EquipmentFaultyId = equipmentFaultyId,
                            };

                            _equipmentFaultyFiles.Add(awesomePemarkFile);
                        }


                    }
                }
            }

            return await SaveDbResult(_unitOfWork);
        }

        public async Task<DbResult> UpdateFileAsync(Guid id, EquipmentFaultyFileStatus status, UploadByBase64Dto uploadByBase64Dto )
        {
          

            var files = await _equipmentFaultyFiles.Where(x => x.Id == id)
                .ToListAsync();

           

                    uploadByBase64Dto.FolderType = UploadFolderType.FactorDoc;
                    var resultUpload = _uploadService.UploadImageAndPdfByBase64(uploadByBase64Dto);
                    if (!string.IsNullOrWhiteSpace(resultUpload))
                    {
                        var fileType = (EquipmentFaultyFileType)int.Parse(uploadByBase64Dto.CustomData);

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

                            var awesomePemarkFile = new DomainClasses.Entity.EquipmentFaulty.EquipmentFaultyFile()
                            {
                                Extension = Path.GetExtension(uploadByBase64Dto.Name),
                                FileName = resultUpload,
                                FileType = fileType,
                                OriginalFileName = uploadByBase64Dto.Name,
                                CreatedOn = DateTime.Now,
                                
                            };

                            _equipmentFaultyFiles.Add(awesomePemarkFile);
                        }

                      
                    }
            

            return await SaveDbResult(_unitOfWork);
        }
        public async Task<DbResult> UpdateAsync(Guid id, EquipmentFaultyFileStatus status)
        {
            var EquipmentFaultyFile = await _equipmentFaultyFiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (EquipmentFaultyFile == null)
                return new DbResult(DbResult.M.NotFound);

            EquipmentFaultyFile.Status = status;

            _unitOfWork.MarkAsChanged(EquipmentFaultyFile);


            return await SaveDbResult(_unitOfWork, EquipmentFaultyFile);
        }

        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var EquipmentFaultyFileFile = await _equipmentFaultyFiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (EquipmentFaultyFileFile == null)
                return new DbResult(DbResult.M.NotFound);

            _unitOfWork.MarkAsSafeDelete(EquipmentFaultyFileFile);


            var result = await SaveDbResult(_unitOfWork, EquipmentFaultyFileFile);

            return result;
        }

   
        public async Task<GetAllTupleResult<EquipmentFaultyFileDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
            Guid? EquipmentFaultyFileId = null, EquipmentFaultyFileStatus? status=null,Guid? EquipmentFaultyId=null)
        {
            var e = Dto();

            if (EquipmentFaultyFileId != null)
                e = e.Where(x => x.Id == EquipmentFaultyFileId).AsQueryable();

            if (EquipmentFaultyId != null)
                e = e.Where(x => x.EquipmentFaultyId == EquipmentFaultyId).AsQueryable();

            if (status != null)
                e = e.Where(x => x.Status == status).AsQueryable();

            return await e.ToGetAllTupleResult<EquipmentFaultyFileDto, EquipmentFaultyFileDto>(getAllTupleDto);
        }

       
        public async Task<IEnumerable<EquipmentFaultyFileDto>> GetFileAsync(
            Guid? EquipmentFaultyFileId = null, EquipmentFaultyFileStatus? status = null, Guid? EquipmentFaultyId = null)
        {
            var e = _equipmentFaultyFiles.AsNoTracking().AsQueryable();

            if (EquipmentFaultyFileId != null)
                e = e.Where(x => x.Id == EquipmentFaultyFileId).AsQueryable();

            if (EquipmentFaultyId != null)
                e = e.Where(x => x.EquipmentFaultyId == EquipmentFaultyId).AsQueryable();

            if (status != null)
                e = e.Where(x => x.Status == status).AsQueryable();

            return await e.ProjectTo<EquipmentFaultyFileDto>(_mapper.ConfigurationProvider).Cacheable().ToListAsync();
        }
        public async Task<EquipmentFaultyFileDto> GetAsync(Guid? id = null,Guid? EquipmentFaultyId=null)
        {
            var e = Dto();

            if (id != null)
                e = e.Where(x => x.Id == id).AsQueryable();
            if (EquipmentFaultyId != null)
                e = e.Where(x => x.EquipmentFaultyId == EquipmentFaultyId).AsQueryable();

            return await e.Cacheable().FirstOrDefaultAsync();

        }

        public List<EquipmentFaultyDto> GetListAsync(GetAllTupleDto getAllTupleDto = null, bool? all = null)
        {
            var e = DtoDocumnetRegister();
            var EquipmentFaulty = new List<EquipmentFaultyDto>();
            if (all == true)
            {
                foreach (var item in e)
                {

                    EquipmentFaulty.Add(item);
                }
                return EquipmentFaulty;
            }
            foreach (var item in e)
            {
                var isRow = _equipmentFaultyFiles.Where(x => x.EquipmentFaultyId == item.Id).ToList();
                if (!isRow.Any())
                {
                    EquipmentFaulty.Add(item);
                }

            }
            return EquipmentFaulty;

        }

        private IQueryable<EquipmentFaultyFileDto> Dto()
        {
            return (from applicantEquipmentFaulty in _equipmentFaultyFiles
                    join EquipmentFaulty in _equipmentFaultys on applicantEquipmentFaulty.EquipmentFaultyId equals EquipmentFaulty.Id

                    select new EquipmentFaultyFileDto()
                    {
                       
                        FileName = applicantEquipmentFaulty.FileName,
                        Extension = applicantEquipmentFaulty.Extension,
                        OriginalFileName = applicantEquipmentFaulty.OriginalFileName,
                        FileType = applicantEquipmentFaulty.FileType,
                        Status = applicantEquipmentFaulty.Status,
                        EquipmentFaultyId = EquipmentFaulty.Id,                   
                        CreatedOn = applicantEquipmentFaulty.CreatedOn,                   
                        Id = applicantEquipmentFaulty.Id,                     
                        
                     
                    }).AsNoTracking()
                .AsQueryable();
        }


        private IQueryable<EquipmentFaultyDto> DtoDocumnetRegister()
        {

            return (from EquipmentFaulty in _equipmentFaultys

                    select new EquipmentFaultyDto()
                    {
                        Id = EquipmentFaulty.Id,
                       
                    }).AsNoTracking()
                .AsQueryable();
        }
    }
}
