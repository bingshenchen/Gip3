using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Repository.Interface
{
    public interface ICustomerRespository
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerById(int? id);
        Customer GetCustomerByIdentityAsync(string id);
        void CreateCustomer(Customer c);
        void UpdateCustomer(Customer c);
        void DeleteCustomer(int id);
        void Save();


    }
}
