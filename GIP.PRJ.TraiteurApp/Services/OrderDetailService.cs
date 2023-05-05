using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly TraiteurAppDbContext _context;

        public OrderDetailService(TraiteurAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"OrderDetailService > CreateOrderDetailAsync: " +
                    $"An error occurred while creating the orderDetail for order {orderDetail.OrderId}", ex);
            }
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(o => o.Id == id);
                if (orderDetail != null)
                {
                    _context.OrderDetails.Remove(orderDetail);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderDetailService > DeleteOrderDetailAsync: " +
                    $"An error occurred while deleting orderDetail with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            try
            {
                return await _context.OrderDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("OrderDetailService > GetAllOrderDetailsAsync: An error occurred while retrieving OrderDetails", ex);
            }
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int id)
        {
            try
            {
                return await _context.OrderDetails.Include(od => od.MenuItem).FirstOrDefaultAsync(od => od.Id == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderDetailService > GetOrderDetailByIdAsync: An error occurred while retrieving orderDetail with ID {id}", ex);
            }
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"OrderDetailService > UpdateOrderDetailAsync: " +
                    $"An error occurred while updating orderDetail with ID {orderDetail.Id}", ex);
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderAsync(int orderId)
        {
            try
            {
                return await _context.OrderDetails.Where(od => od.OrderId == orderId).Include(od => od.MenuItem).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderDetailService > GetOrderDetailsByOrderAsync: " +
                    $"An error occurred while retrieving orderdetails by order id {orderId}", ex);
            }
        }

        public async Task<bool> AnyMenuItemAsync(int menuItemId)
        {
            try
            {
                return await _context.OrderDetails.AnyAsync(od => od.MenuItemId == menuItemId);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderDetailService > AnyMenuItemAsync: " +
                    $"An error occurred while retrieving orderdetails by order id {menuItemId}", ex);
            }
        }
    }
}
