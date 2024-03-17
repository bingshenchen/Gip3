using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class CustomerService : ICustomerService
    {
        TraiteurAppDbContext _context;
        public CustomerService(TraiteurAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"CustomerService > CreateCustomerAsync: An error occurred while creating the customer {customer.Name}", ex);
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(o => o.Id == id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"CustomerService > DeleteCustomerAsync: " +
                    $"An error occurred while deleting customer with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            try
            {
                return await _context.Customers.Include(c => c.IdentityUser).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("CustomerService > GetAllCustomersAsync: An error occurred while retrieving Customers", ex);
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                return await _context.Customers.Include(c => c.IdentityUser).FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"CustomerService > GetCustomerByIdAsync: An error occurred while retrieving customer with ID {id}", ex);
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"CustomerService > UpdateCustomerAsync: " +
                    $"An error occurred while updating customer with ID {customer.Id}", ex);
            }
        }

        public async Task<Customer> GetCustomerByIdentityAsync(string id)
        {
            try
            {
                return await _context.Customers.Include(c => c.IdentityUser).FirstOrDefaultAsync(c => c.IdentityUserId == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"CustomerService > GetCustomerByIdentityAsync: " +
                    $"An error occurred while retrieving customer with Identity id {id}", ex);
            }
        }
    }
}
