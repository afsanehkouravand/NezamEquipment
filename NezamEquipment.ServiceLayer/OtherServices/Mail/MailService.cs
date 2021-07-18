using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer.Entity.Setting;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml;

namespace NezamEquipment.ServiceLayer.OtherServices.Mail
{
    public class MailService : IMailService
    {
        public bool Send(string subject, string message, string sendTo)
        {
            var mailMessage = new MailMessage();
            try
            {
                var settingService = DependencyResolver.Current.GetService<ISettingService>();
                var setting = settingService.Get<SettingMailXml>();

                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.From = new MailAddress(setting.From, setting.Title);
                mailMessage.To.Add(sendTo);
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = setting.ClientHost;
                    var port = setting.Port;
                    if (port > 0)
                        smtpClient.Port = port;
                    smtpClient.EnableSsl = setting.EnableSsl;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(setting.UserName, setting.Password);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(mailMessage);
                }

                mailMessage.Dispose();

                return true;
            }
            catch (Exception)
            {
                mailMessage.Dispose();
            }

            return false;
        }

        public async Task<bool> SendAsync(string subject, string message, string sendTo)
        {
            var mailMessage = new MailMessage();
            try
            {
                var settingService = DependencyResolver.Current.GetService<ISettingService>();
                var setting = settingService.Get<SettingMailXml>();

                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.From = new MailAddress(setting.From, setting.Title);
                mailMessage.To.Add(sendTo);
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = setting.ClientHost;
                    var port = setting.Port;
                    if (port > 0)
                        smtpClient.Port = port;
                    smtpClient.EnableSsl = setting.EnableSsl;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(setting.UserName, setting.Password);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await smtpClient.SendMailAsync(mailMessage);
                }

                mailMessage.Dispose();

                return true;
            }
            catch (Exception)
            {
                mailMessage.Dispose();
            }

            return false;
        }

    }
}
