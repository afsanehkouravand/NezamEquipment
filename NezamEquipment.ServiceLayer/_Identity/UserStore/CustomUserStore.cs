using System;
using Microsoft.AspNet.Identity.EntityFramework;
using NezamEquipment.DataLayer.DbContext;
using NezamEquipment.DomainClasses.Identity;

namespace NezamEquipment.ServiceLayer._Identity.UserStore
{
    public class CustomUserStore :
        UserStore<DomainClasses.Identity.User, DomainClasses.Identity.Role, Guid, UserLogin, UserRole, UserClaim>,
        ICustomUserStore
    {
        public CustomUserStore(NezamEquipmentDbContext context)
            : base(context)
        {
        }

    }
}