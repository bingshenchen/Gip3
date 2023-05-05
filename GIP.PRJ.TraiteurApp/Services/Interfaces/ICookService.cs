using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface ICookService
    {
        Task<IEnumerable<Cook>> GetAllCooksAsync();
        Task<Cook> GetCookByIdAsync(int id);
        Task<Cook> GetCookByIdentityAsync(string id);
        Task CreateCookAsync(Cook cook);
        Task UpdateCookAsync(Cook cook);
        Task DeleteCookAsync(int id);
    }
}
