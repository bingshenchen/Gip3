using GIP.PRJ.TraiteurApp.Models;

namespace GIP.PRJ.TraiteurApp.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderAsync(int orderId);
        Task<bool> AnyMenuItemAsync(int menuItemId);
        Task<OrderDetail> GetOrderDetailByIdAsync(int id);
        Task CreateOrderDetailAsync(OrderDetail orderDetail);
        Task UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task DeleteOrderDetailAsync(int id);
    }
}
