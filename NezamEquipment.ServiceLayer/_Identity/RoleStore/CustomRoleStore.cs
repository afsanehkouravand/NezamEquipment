using System;
using Microsoft.AspNet.Identity;

namespace NezamEquipment.ServiceLayer._Identity.RoleStore
{
    public class CustomRoleStore : ICustomRoleStore
    {
        private readonly IRoleStore<DomainClasses.Identity.Role, Guid> _roleStore;

        public CustomRoleStore(IRoleStore<DomainClasses.Identity.Role, Guid> roleStore)
        {
            _roleStore = roleStore;
        }
    }
}