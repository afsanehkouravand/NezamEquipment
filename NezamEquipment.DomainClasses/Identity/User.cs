using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NezamEquipment.DomainClasses.Entity.Employees;
using NezamEquipment.DomainClasses.Entity.Equipment;
using NezamEquipment.DomainClasses.Entity.EquipmentFaulty;
using NezamEquipment.DomainClasses.Identity.Enum;

namespace NezamEquipment.DomainClasses.Identity
{
    public class User : IdentityUser<Guid, UserLogin, UserRole, UserClaim>
    {
        #region GenerateUserIdentityAsync

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #endregion

        #region Properties

        public AreaType AreaType { get; set; }

        [StringLength(200)]
        public string FirstName { get; set; }

        [StringLength(200)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Position { get; set; }
       
        public DateTime? LastLogin { get; set; }

        #endregion

        #region Navigation Properties
        public virtual ICollection<Employe> Employes { get; set; }
        public virtual ICollection<Equipment> Equipments{ get; set; }
        public virtual ICollection<EquipmentFaulty> EquipmentFaulties { get; set; }


        #endregion

    }
}