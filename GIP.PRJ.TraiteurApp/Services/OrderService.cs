using GIP.PRJ.TraiteurApp.Models;
using GIP.PRJ.TraiteurApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly TraiteurAppDbContext _context;

        public OrderService(TraiteurAppDbContext context)
        {
            _context = context;
        }
    
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _context.Orders.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("OrderService > GetAllOrdersAsync: An error occurred while retrieving orders", ex);
            }
        }
        public async Task<IEnumerable<Order>> GetOrdersFromDateAsync(DateTime dateTime)
        {
            try
            {
                return await _context.Orders.Where(o => o.OrderedOn >= dateTime).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderService > GetOrdersFromDateAsync: " +
                    $"An error occurred while retrieving orders from date {dateTime}", ex);
            }
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                return await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderService > GetOrderByIdAsync: An error occurred while retrieving order with ID {id}", ex);
            }
        }

        public async Task CreateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"OrderService > CreateOrderAsync: An error occurred while creating order", ex);
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // logging
                throw new Exception($"OrderService > UpdateOrderAsync: An error occurred while updating order with ID {order.Id}", ex);
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"OrderService > RemoveOrderAsync: An error occurred while deleting order with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            try
            {
                return await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("OrderService > GetOrdersByCustomerAsync: An error occurred while retrieving orders", ex);
            }
        }


        public async Task<IEnumerable<Order>> GetOrdersByCookAsync(int? cookId)
        {
            try
            {
                return await _context.Orders.Where(o => o.CookId == cookId && 
                    o.IsPaid == false && o.Status != OrderStatus.Finished).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("OrderService > GetOrdersByCookAsync: An error occurred while retrieving orders", ex);
            }
        }

        public async Task<int> OrderCountByCustomer(int customerId)
        {
            try
            {
                return await _context.Orders.CountAsync(o => o.CustomerId == customerId && o.IsPaid == true);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("OrderService > OrderCountByCustomer:" +
                    $" An error occurred while counting orders for customer {customerId}", ex);
            }
        }

        public async Task<int> OrderCountByCook(int cookId)
        {
            try
            {
                return await _context.Orders.CountAsync(o => o.CookId == cookId);
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("OrderService > OrderCountByCook:" +
                    $" An error occurred while counting orders for cook {cookId}", ex);
            }
        }
    }
}
