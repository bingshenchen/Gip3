using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class MenuItemService : IMenuItemService
    {
        TraiteurAppDbContext _context;
        public MenuItemService(TraiteurAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateMenuItemAsync(MenuItem menuItem)
        {
            try
            {
                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"MenuItemService > CreateMenuItemAsync: " +
                    $"An error occurred while creating the MenuItem {menuItem.Description}", ex);
            }
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            try
            {
                var menuItem = await _context.MenuItems.FirstOrDefaultAsync(o => o.Id == id);
                if (menuItem != null)
                {
                    _context.MenuItems.Remove(menuItem);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"MenuItemService > DeleteMenuItemAsync: " +
                    $"An error occurred while deleting menuItem with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
            try
            {
                return await _context.MenuItems.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("MenuItemService > GetAllMenuItemsAsync: An error occurred while retrieving MenuItems", ex);
            }
        }

        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
            try
            {
                return await _context.MenuItems.Include(mi => mi.MenuItemCategory).FirstOrDefaultAsync(mi => mi.Id == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"MenuItemService > GetMenuItemByIdAsync: An error occurred while retrieving menuItem with ID {id}", ex);
            }
        }

        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            try
            {
                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"MenuItemService > UpdateMenuItemAsync: " +
                    $"An error occurred while updating menuItem with ID {menuItem.Id}", ex);
            }
        }
    }
}
