using AutoMapper;
using NezamEquipment.AutoMapper.Extentions;
using NezamEquipment.DomainClasses.Entity.Equipment;
using NezamEquipment.ServiceLayer.Entity.Segment.Dto;

namespace NezamEquipment.AutoMapper.Profiles
{
    public class SegmentProfile : Profile
    {
        public SegmentProfile()
        {

            CreateMap<Segment, SegmentDto>()
                .IgnoreAllNonExisting();

            CreateMap<SegmentDto, Segment>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}