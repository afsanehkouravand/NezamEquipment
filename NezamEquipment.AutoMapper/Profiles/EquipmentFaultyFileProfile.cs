using AutoMapper;
using NezamEquipment.AutoMapper.Extentions;

using NezamEquipment.DomainClasses.Entity.EquipmentFaulty;

using NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto;
using NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile.Dto;

namespace NezamEquipment.AutoMapper.Profiles
{
    public class EquipmentFaultyFileProfile : Profile
    {
        public EquipmentFaultyFileProfile()
        {

            CreateMap<EquipmentFaultyFile, EquipmentFaultyFileDto>()
                .IgnoreAllNonExisting();

            CreateMap<EquipmentFaultyFileDto, EquipmentFaultyFile>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}