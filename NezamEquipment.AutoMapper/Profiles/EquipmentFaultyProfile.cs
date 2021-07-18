using AutoMapper;
using NezamEquipment.AutoMapper.Extentions;

using NezamEquipment.DomainClasses.Entity.EquipmentFaulty;

using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;

namespace NezamEquipment.AutoMapper.Profiles
{
    public class EquipmentFaultyProfile : Profile
    {
        public EquipmentFaultyProfile()
        {

            CreateMap<EquipmentFaulty, EquipmentFaultyDto>()
                .IgnoreAllNonExisting();

            CreateMap<EquipmentFaultyDto, EquipmentFaulty>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}