using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NezamEquipment.ServiceLayer._Identity.User;

namespace NezamEquipment.ServiceLayer._Identity.SignIn
{
    public class SignInManager :
        SignInManager<DomainClasses.Identity.User, Guid>, ISignInManager
    {
        private readonly UserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public SignInManager(UserManager userManager,
                                        IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }
    }
}