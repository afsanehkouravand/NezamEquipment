using AutoMapper;
using NezamEquipment.AutoMapper.Extentions;
using NezamEquipment.DomainClasses.Entity.Equipment;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;

namespace NezamEquipment.AutoMapper.Profiles
{
    public class EquipmentProfile : Profile
    {
        public EquipmentProfile()
        {

            CreateMap<Equipment, EquipmentDto>()
                .IgnoreAllNonExisting();

            CreateMap<EquipmentDto, Equipment>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}