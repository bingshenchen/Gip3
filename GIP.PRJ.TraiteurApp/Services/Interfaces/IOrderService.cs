using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersFromDateAsync(DateTime dateTime);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByCookAsync(int? cookId);
        Task<Order> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<int> OrderCountByCustomer(int customerId);
        Task<int> OrderCountByCook(int cookId);
    }
}
