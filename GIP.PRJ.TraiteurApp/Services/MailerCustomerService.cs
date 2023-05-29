using GIP.PRJ.TraiteurApp.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using GIP.PRJ.TraiteurApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class MailerCustomerService : IMailerCustomerService
    {
        private readonly ILogger<MailerWorkerService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MailerCustomerService(ILogger<MailerWorkerService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task SendConfirmationMailAsync(string toEmailAddress, string emailSubject, string emailBody)
        {
            try { 
            var fromAddress = new MailAddress("lekkerbekgip3@outlook.com", "Lekker Bek");
            var toAddress = new MailAddress(toEmailAddress);

            const string fromPassword = "gip3groep21";

                var OutLookSmtp = new SmtpClient
                {
                    Host = "smtp.office365.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    // Credentials = new NetworkCredential(fromAddress.Address, encryptedPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = emailSubject,
                    Body = emailBody
                })
                {
                    OutLookSmtp.Send(message);
                }
            }
            catch(SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public async Task DoWork()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TraiteurAppDbContext>();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                //klanten die mail moeten krijgen
                var customers = context.Customers.ToList();
                var customerToConfirm = customers.ToList();

                // herinneringsmail sturen naar klanten
                foreach (var customer in customerToConfirm)
                {
                    var emailSubject = "Je aanmelding is bevestigd.(do-not-answer)";
                    var emailMessage = $"Beste {customer.Name},\\n\\nBij deze is je aanmelding bij Lekkerbek bevestigd.";
                    await SendConfirmationMailAsync(customer.Name, emailSubject, emailMessage);

                    context.Update(customer);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
