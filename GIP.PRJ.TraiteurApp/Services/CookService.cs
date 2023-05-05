using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class CookService : ICookService
    {
        TraiteurAppDbContext _context;
        public CookService(TraiteurAppDbContext context)
        {
            _context = context;
        }
        public async Task CreateCookAsync(Cook cook)
        {
            try
            {
                _context.Cooks.Add(cook);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"CookService > CreateCookAsync: " +
                    $"An error occurred while creating the customer {cook.ChefName}", ex);
            }
        }

        public async Task DeleteCookAsync(int id)
        {
            try
            {
                var customer = await _context.Cooks.FirstOrDefaultAsync(o => o.Id == id);
                if (customer != null)
                {
                    _context.Cooks.Remove(customer);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"CookService > DeleteCookAsync: " +
                    $"An error occurred while deleting cook with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Cook>> GetAllCooksAsync()
        {
            try
            {
                return await _context.Cooks.Include(c => c.IdentityUser).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("CookService > GetAllCooksAsync: An error occurred while retrieving Cooks", ex);
            }
        }

        public async Task<Cook> GetCookByIdAsync(int id)
        {
            try
            {
                return await _context.Cooks.Include(c => c.IdentityUser).FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"CookService > GetCookByIdAsync: An error occurred while retrieving cook with ID {id}", ex);
            }
        }

        public async Task<Cook> GetCookByIdentityAsync(string id)
        {
            try
            {
                return await _context.Cooks.Include(c => c.IdentityUser).FirstOrDefaultAsync(c => c.IdentityUserId == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"CookService > GetCookByIdentityAsync: " +
                    $"An error occurred while retrieving cook with Identity id {id}", ex);
            }
        }

        public async Task UpdateCookAsync(Cook cook)
        {
            try
            {
                _context.Cooks.Update(cook);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"CookService > UpdateCookAsync: " +
                    $"An error occurred while updating customer with ID {cook.Id}", ex);
            }
        }
    }
}
