namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IMailService
    {
        void SendMail(string from, string to, string subject, string body);
    }
}
