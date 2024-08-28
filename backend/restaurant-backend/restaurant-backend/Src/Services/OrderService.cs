using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Src.Services
{
    public class OrderService : IOrderService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMemoryCache _cache;

        public OrderService(RestaurantDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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
                // Define a cache key for the specific order.
                string cacheKey = $"Order_{orderId}";

                // Try to get the order from the cache.
                if (!_cache.TryGetValue(cacheKey, out Order cachedOrder))
                {
                    // If the order is not found in the cache, retrieve it from the database.
                    var order = await _context.Orders.FindAsync(orderId);

                    if (order == null)
                    {
                        throw new KeyNotFoundException($"Order with ID {orderId} not found.");
                    }

                    // Define cache options, such as expiration.
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };

                    // Cache the order for future use.
                    _cache.Set(cacheKey, order, cacheOptions);

                    return order;
                }

                return cachedOrder;
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
                // Define a cache key for all orders.
                string cacheKey = "AllOrders";

                // Try to get the orders from the cache.
                if (!_cache.TryGetValue(cacheKey, out IEnumerable<Order> cachedOrders))
                {
                    // If the orders are not found in the cache, retrieve them from the database.
                    var orders = await _context.Orders.ToListAsync();

                    // Define cache options, such as expiration.
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };

                    // Cache the orders for future use.
                    _cache.Set(cacheKey, orders, cacheOptions);

                    return orders;
                }

                return cachedOrders;
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
                // Define a cache key for orders by table number.
                string cacheKey = $"Orders_Table_{tableNumber}";

                // Try to get the orders from the cache.
                if (!_cache.TryGetValue(cacheKey, out IEnumerable<Order> cachedOrders))
                {
                    // If the orders are not found in the cache, retrieve them from the database.
                    var orders = await _context.Orders
                        .Where(o => o.TableNumber == tableNumber)
                        .ToListAsync();

                    // Define cache options, such as expiration.
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };

                    // Cache the orders for future use.
                    _cache.Set(cacheKey, orders, cacheOptions);

                    return orders;
                }

                return cachedOrders;
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
