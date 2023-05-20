namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IMailerWorkerService
    {
        Task DoWorkAsync();
        Task SendReminderEmailsAsync(string toEmailAddress, string emailSubject, string emailMessage);
    }
}
