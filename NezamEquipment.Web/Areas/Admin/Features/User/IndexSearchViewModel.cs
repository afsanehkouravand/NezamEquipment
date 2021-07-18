using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Identity.Enum;

namespace NezamEquipment.Web.Areas.Admin.Features.User
{
    public class AdminUserIndexSearchViewModel
    {
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "نقض")]
        public Guid? RoleId { get; set; }
        public IDictionary<string, string> DropDownRole { get; set; }

        [Display(Name = "کنترل پنل")]
        public AreaType? AreaType { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string Fullname { get; set; }

        [Display(Name = "موقعیت")]
        public string Position { get; set; }

    }
}
