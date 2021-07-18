using AutoMapper;
using NezamEquipment.AutoMapper.Extentions;
using NezamEquipment.DomainClasses.Entity.Employees;
using NezamEquipment.ServiceLayer.Entity.Employees.Dto;

namespace NezamEquipment.AutoMapper.Profiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {

            CreateMap<Employe, EmployeDto>()
                .IgnoreAllNonExisting();

            CreateMap<EmployeDto, Employe>()
                .IgnoreAllNonExisting();

        }

        public override string ProfileName => GetType().Name;
    }
}