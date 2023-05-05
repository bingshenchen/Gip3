namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface ITimeSlotService
    {
        Task<Dictionary<string, string>> GetTimeSlotDictionary();
        Task<bool> OrderIsLocked(int orderid);
    }
}
