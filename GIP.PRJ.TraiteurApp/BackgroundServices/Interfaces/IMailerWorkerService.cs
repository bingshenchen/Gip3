namespace GIP.PRJ.TraiteurApp.BackgroundServices.Interfaces
{
    public interface IMailerWorkerService
    {
        Task DoWorkAsync();
        Task SendReminderEmailsAsync(string toEmailAddress, string emailSubject, string emailMessage);
    }
}
