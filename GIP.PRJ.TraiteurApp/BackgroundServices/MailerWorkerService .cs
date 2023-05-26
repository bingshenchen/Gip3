using GIP.PRJ.TraiteurApp.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using GIP.PRJ.TraiteurApp.Models;
using Microsoft.EntityFrameworkCore;
using GIP.PRJ.TraiteurApp.BackgroundServices.Interfaces;

namespace GIP.PRJ.TraiteurApp.BackgroundServices
{
    public class MailerWorkerService : IMailerWorkerService
    {
        private readonly ILogger<MailerWorkerService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MailerWorkerService(ILogger<MailerWorkerService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task SendReminderEmailsAsync(string toEmailAddress, string emailSubject, string emailMessage)
        {
            try
            {
                var fromAddress = new MailAddress("lekkerbekgip3@outlook.com", "Lekker Bek");
                var toAddress = new MailAddress(toEmailAddress);

                // var encryptedPassword = AesOperation.EncryptString(_encryptionKey, "gip3groep21");
                const string fromPassword = "gip3groep21"; // Tijdelijke mailbox

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

                /*
                var GmailSmtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587
                }

                var YahooSmtp = new SmtpClient
                {
                    Host = "smtp.mail.yahoo.com",
                    Port = 587
                }

                var iCloudSmtp = new SmtpClient
                {
                    Host = "smtp.mail.me.com",
                    Port = 587
                }

                var QMailSmtp = new SmtpClient
                {
                    Host = "smtp.exmail.qq.com",
                    Port = 25
                }

                var HotmaiSmtp = new SmtpClient
                {
                    Host = "smtp.live.com",
                    Port = 587
                }

                */

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = emailSubject,
                    Body = emailMessage
                })
                {
                    OutLookSmtp.Send(message);
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());

            }
        }
        public async Task DoWorkAsync()
        {
            // Huidige tijd
            var currentTime = DateTime.Now.TimeOfDay;

            // Tijdlimiet voor herinnering (3 uur vooraf)
            var reminderTime = currentTime.Add(TimeSpan.FromHours(3));

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TraiteurAppDbContext>();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                // Haal de klanten op die een herinneringse-mail moeten ontvangen
                var reminders = context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.ReminderSent.Equals(false))
                    .ToList();


                /*Customers*/

                var allCustomersWithOrders = reminders
                .Where(o =>
                {
                    TimeSpan orderTime;
                    return TimeSpan.TryParse(o.TimeSlot, out orderTime) && CheckTimeSlot(orderTime, currentTime, reminderTime);
                })
                .ToList();

                // Verzend herinneringse-mails naar klanten
                foreach (var order in allCustomersWithOrders)
                {

                    var emailSubject = "Herinnering: Haal uw gerechten na 3 uur op (do-not-answer)";
                    var emailMessage = $"Beste {order.Customer.Name},\n\nDit is een vriendelijke herinnering dat " +
                        $"u uw gerechten binnenkort moet ophalen. Uw bestelling met ID {order.Id} is bijna beschikbaar " +
                        $"voor ophalen. Over ongeveer drie uur kunt u uw pakket ophalen bij onze locatie.\r\n\r\n Bedankt" +
                        $" voor uw aankoop en we kijken uit naar uw bezoek.\r\n\r\n Met vriendelijke groeten,\r\n Het team21 van LekkerBek";

                    await SendReminderEmailsAsync(order.Customer.EmailAddress, emailSubject, emailMessage);

                    order.ReminderSent = true;
                    await context.SaveChangesAsync();
                }
            }
        }
        private bool CheckTimeSlot(TimeSpan timeSlot, TimeSpan currentTime, TimeSpan reminderTime)
        {
            return timeSlot >= currentTime && timeSlot <= reminderTime;
        }
    }
}
