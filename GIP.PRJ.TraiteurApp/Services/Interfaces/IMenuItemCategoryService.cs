using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IMenuItemCategoryService
    {
        Task<IEnumerable<MenuItemCategory>> GetAllMenuItemCategoriesAsync();
        Task<MenuItemCategory> GetMenuItemCategoryByIdAsync(int id);
        Task CreateMenuItemCategoryAsync(MenuItemCategory menuItem);
        Task UpdateMenuItemCategoryAsync(MenuItemCategory menuItem);
        Task DeleteMenuItemCategoryAsync(int id);
    }
}
