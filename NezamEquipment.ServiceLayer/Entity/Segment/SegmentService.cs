using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DataLayer.UnitOfWork;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.ServiceLayer.Entity.Segment.Dto;

namespace NezamEquipment.ServiceLayer.Entity.Segment
{
    public class Segmentervice : BaseSaveDbResult, ISegmentService
    {
       
        private readonly IDbSet<DomainClasses.Entity.Equipment.Segment> _segments;
     
        private readonly IUnitOfWork<NezamEquipmentDbContext> _unitOfWork;
        private readonly IMapper _mapper;
       
        public Segmentervice(
            IUnitOfWork<NezamEquipmentDbContext> unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _segments = _unitOfWork.Set<DomainClasses.Entity.Equipment.Segment>();
         
        }

        public async Task<DbResult> AddAsync(SegmentDto SegmentDto)
        {
          
            var Segment = _mapper.Map<DomainClasses.Entity.Equipment.Segment>(SegmentDto);
           
            Segment.CreatedOn = DateTime.Now;
            
            _segments.Add(Segment);

            var result = await SaveDbResult(_unitOfWork, Segment);


            return result;
        }

        public async Task<DbResult> EditAsync(SegmentDto dto)
        {
            var Segment = await _segments.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (Segment == null)
                return new DbResult(DbResult.M.NotFound);
           
            Segment.Type = dto.Type;
           
            _unitOfWork.MarkAsChanged(Segment);
            
            
           var result=   await SaveDbResult(_unitOfWork, Segment);
            return result;


        }

        public async Task<GetAllTupleResult<SegmentDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
           
            Guid? id = null,DateTime? createdOn = null, DateTime? toCreatedOn = null,int? year=null)
        {
            var e = Dto();
            
            if (toCreatedOn != null)
            {
                var end = new DateTime(toCreatedOn.Value.Year, toCreatedOn.Value.Month, toCreatedOn.Value.Day, 0, 0, 0);
                e = e.Where(x => x.CreatedOn < end).AsQueryable();
            }
          


            return await e.ToGetAllTupleResult<SegmentDto, SegmentDto>(getAllTupleDto);
        }

     

        public async Task<SegmentDto> GetAsync(Guid? id = null, DateTime? createdOn = null,
            int? year = null)
        {
            var e = Dto();
            if (id != null)
            {
                e = e.Where(x => x.Id == id).AsQueryable();
            }
          
            if (createdOn != null)
            {
                var start = new DateTime(createdOn.Value.Year, createdOn.Value.Month, createdOn.Value.Day, 0, 0, 0);
                var end = start.AddDays(1);
                e = e.Where(x => x.CreatedOn >= start && x.CreatedOn < end).AsQueryable();
            }
           
           

            return await e.Cacheable().FirstOrDefaultAsync();
        }

        private IQueryable<SegmentDto> Dto()
        {
            return (from transfer in _segments
                   
                    select new SegmentDto()
                    {
                       Type = transfer.Type,
                        CreatedOn = transfer.CreatedOn,
                      Id = transfer.Id,

                    }).AsNoTracking()
                .AsQueryable();
        }

        public async Task<DbResult> DeleteAsync(Guid id)
        {
            var transfer = await _segments.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (transfer == null)
                return new DbResult(DbResult.M.NotFound);

            _unitOfWork.MarkAsSafeDelete(transfer);

            return await SaveDbResult(_unitOfWork);
        }

     
    }
}
