namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IMailerCustomerService
    {
        Task SendConfirmationMailAsync(string toMailAddress, string emailSubject, string emailBody);
        Task DoWork();
    }
}
