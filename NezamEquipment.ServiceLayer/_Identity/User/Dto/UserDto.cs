using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Identity.Enum;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;

namespace NezamEquipment.ServiceLayer._Identity.User.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Display(Name = "نام کاربری")]
        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required]
        [EmailAddress]
        [StringLength(250)]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(200)]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [StringLength(200)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string Fullname { get; set; }

        [StringLength(200)]
        [Display(Name = "موقعیت")]
        public string Position { get; set; }

        [Display(Name = "نقش ها")]
        public IList<RoleDto> Roles { get; set; }

        [Display(Name = "پنل مدیریت")]
        public AreaType AreaType { get; set; }

        [Display(Name = "آخرین ورود")]
        public DateTime? LastLogin { get; set; }

    }
}
