using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NezamEquipment.ServiceLayer.OtherServices.Mail;

namespace NezamEquipment.ServiceLayer._Identity
{
    public class EmailService : IIdentityMessageService
    {
        private readonly IMailService _mailService;

        public EmailService(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            // send mail via service
            await _mailService.SendAsync(message.Subject, message.Body, message.Destination);
        }
    }
}