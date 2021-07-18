using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using NezamEquipment.DomainClasses.Identity;
using NezamEquipment.ServiceLayer._Identity;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.RoleStore;
using NezamEquipment.ServiceLayer._Identity.SignIn;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer._Identity.UserStore;
using StructureMap;
using StructureMap.Web;

namespace NezamEquipment.Web.DependencyResolution.Registery
{
    public class AspNetIdentityRegistery : Registry
    {
        public AspNetIdentityRegistery()
        {

            For<IUserStore<User, Guid>>().Use<CustomUserStore>();

            For<IRoleStore<Role, Guid>>().Use<RoleStore<Role, Guid, UserRole>>();

            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<ISignInManager>().Use<SignInManager>();

            For<IRoleManager>().Use<RoleManager>();

            // map same interface to different concrete classes
            For<IIdentityMessageService>().Use<SmsService>();
            For<IIdentityMessageService>().Use<EmailService>();

            For<IUserManager>().Use<UserManager>()
               .Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
               .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
               .Setter(userManager => userManager.SmsService).Is<SmsService>()
               .Setter(userManager => userManager.EmailService).Is<EmailService>();

            For<UserManager>()
               .Use(context => (UserManager)context.GetInstance<IUserManager>());

            For<ICustomRoleStore>().Use<CustomRoleStore>();

            For<ICustomUserStore>().Use<CustomUserStore>();

            For<IDataProtectionProvider>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() => Startup.DataProtectionProvider);

        }
    }

}
