using AutoMapper;
using NezamEquipment.AutoMapper.Extentions;
using NezamEquipment.DomainClasses.Entity.Equipment;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;

namespace NezamEquipment.AutoMapper.Profiles.Identity
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserNameAndIdDto>()
                .IgnoreAllNonExisting();

            CreateMap<User, UserDto>()
                .IgnoreAllNonExisting();

            CreateMap<UserDto, User>()
                .IgnoreAllNonExisting();

            CreateMap<UserRole, RoleDto>()
                .IgnoreAllNonExisting();

            CreateMap< RoleDto , UserRole>()
                .IgnoreAllNonExisting();
           
            CreateMap<RoleAccess, RoleAccessDto>()
              .IgnoreAllNonExisting();
            CreateMap<RoleAccessDto, RoleAccess>()
               .IgnoreAllNonExisting();
           

        }


        public override string ProfileName => GetType().Name;
    }
}