using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class MailService : IMailService
    {
        public void SendMail(string from, string to, string subject, string body)
        {
            /// sending mail using the build in smtp client 
            /// Replace correct values for smtpUrl, from@domain and password
            /// Example:
            /// smtpUrl: relay.proximus.be
            /// from@domain: gip.teamx@proximus.be
            /// password: UcllGip2023,,
            SmtpClient smtpClient = new SmtpClient("smtpUrl", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("from@domain", "password")
            };
            MailMessage mailMessage = new MailMessage(from, to, subject, body);
            mailMessage.IsBodyHtml = true;
            try
            {
                smtpClient.Send(mailMessage);
               
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"MailService > SendMail: An error occurred while sending mail to {to}", ex);
            }
        }
    }
}
