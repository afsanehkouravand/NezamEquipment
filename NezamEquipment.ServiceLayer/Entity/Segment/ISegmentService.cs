using System;
using System.Threading.Tasks;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.ServiceLayer.Entity.Segment.Dto;


namespace NezamEquipment.ServiceLayer.Entity.Segment
{
    public interface ISegmentService
    {
        Task<DbResult> AddAsync(SegmentDto segmentDto);
        Task<DbResult> EditAsync(SegmentDto dto);

        Task<GetAllTupleResult<SegmentDto>> GetAllAsync(GetAllTupleDto getAllTupleDto = null,
           
            Guid? id = null,DateTime? createdOn = null, DateTime? toCreatedOn = null,int? year=null);

        Task<SegmentDto> GetAsync(Guid? id = null, DateTime? createdOn = null,
            int? year = null);

        Task<DbResult> DeleteAsync(Guid id);
    }
}