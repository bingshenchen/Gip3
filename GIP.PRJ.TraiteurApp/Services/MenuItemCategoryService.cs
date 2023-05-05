using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class MenuItemCategoryService : IMenuItemCategoryService
    {
        TraiteurAppDbContext _context;
        public MenuItemCategoryService(TraiteurAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateMenuItemCategoryAsync(MenuItemCategory menuItemCategory)
        {
            try
            {
                _context.MenuItemCategories.Add(menuItemCategory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"MenuItemCategoryService > CreateMenuItemCategoryAsync: " +
                    $"An error occurred while creating the MenuItemCategory {menuItemCategory.Name}", ex);
            }
        }

        public async Task DeleteMenuItemCategoryAsync(int id)
        {
            try
            {
                var menuItemCategory = await _context.MenuItemCategories.FirstOrDefaultAsync(o => o.Id == id);
                if (menuItemCategory != null)
                {
                    _context.MenuItemCategories.Remove(menuItemCategory);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"MenuItemCategoryService > DeleteMenuCategoryItemAsync: " +
                    $"An error occurred while deleting menuItemCategory with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<MenuItemCategory>> GetAllMenuItemCategoriesAsync()
        {
            try
            {
                return await _context.MenuItemCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("MenuItemCategoryService > GetAllMenuItemCategoriesAsync: " +
                    "An error occurred while retrieving MenuItemCategories", ex);
            }
        }

        public async Task<MenuItemCategory> GetMenuItemCategoryByIdAsync(int id)
        {
            try
            {
                return await _context.MenuItemCategories.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"MenuItemCategoryService > GetMenuItemCategoryByIdAsync:" +
                    $" An error occurred while retrieving menuItemCategory with ID {id}", ex);
            }
        }

        public async Task UpdateMenuItemCategoryAsync(MenuItemCategory menuItemCategory)
        {
            try
            {
                _context.MenuItemCategories.Update(menuItemCategory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"MenuItemCategoryService > UpdateMenuItemCategoryAsync: " +
                    $"An error occurred while updating menuItemCategory with ID {menuItemCategory.Id}", ex);
            }
        }
    }
}
