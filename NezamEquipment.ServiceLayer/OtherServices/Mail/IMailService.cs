using System.Threading.Tasks;

namespace NezamEquipment.ServiceLayer.OtherServices.Mail
{
    public interface IMailService
    {
        bool Send(string subject, string message, string sendTo);
        Task<bool> SendAsync(string subject, string message, string sendTo);
    }
}