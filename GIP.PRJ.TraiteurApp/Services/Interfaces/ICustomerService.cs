using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> GetCustomerByIdentityAsync(string id);
        Task CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
    }
}
