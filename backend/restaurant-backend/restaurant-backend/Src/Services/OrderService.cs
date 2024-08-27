using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Src.Services
{
    public class OrderService : IOrderService
    {
        private readonly RestaurantDbContext _context;

        public OrderService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order newOrder)
        {
            try
            {
                await _context.Orders.AddAsync(newOrder);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the order.", ex);
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");
                }

                return order;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the order by ID.", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _context.Orders.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all orders.", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByTableNumberAsync(int tableNumber)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.TableNumber == tableNumber)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving orders by table number.", ex);
            }
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");
                }

                order.OrderStatus = newStatus;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the order status.", ex);
            }
        }

        public async Task UpdateTotalPriceAsync(int orderId, double newTotalPrice)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");
                }

                order.TotalPrice = newTotalPrice;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the total price.", ex);
            }
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while canceling the order.", ex);
            }
        }

        public async Task<Order> GetMostRecentOrderForTableAsync(int tableNumber)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.TableNumber == tableNumber)
                    .OrderByDescending(o => o.OrderTime)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the most recent order for the table.", ex);
            }
        }

        

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.OrderStatus == "Pending")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving pending orders.", ex);
            }
        }
    }

}
