using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using NezamEquipment.DomainClasses.Identity.Enum;

namespace NezamEquipment.DomainClasses.Identity
{
    public class Role : IdentityRole<Guid, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
        public AreaType AreaType { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}