using AutoMapper;
using NezamEquipment.AutoMapper.Extentions;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;

namespace NezamEquipment.AutoMapper.Profiles.Identity
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .IgnoreAllNonExisting();

            CreateMap<RoleDto, Role>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}