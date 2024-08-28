using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.OrderDTOS;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Src.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMemoryCache _cache;

        public OrderItemService(RestaurantDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task AddOrderItemAsync(AddOrderItemRequestDTO dto)
        {
            try
            {
                var orderItem = new OrderItem
                {
                    OrderID = dto.OrderID,
                    MenuItemID = dto.MenuItemID,
                    Quantity = dto.Quantity
                };

                await _context.OrderItems.AddAsync(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the order item.", ex);
            }
        }

        public async Task<bool> CheckIfOrderItemExistsAsync(int orderItemID)
        {
            try
            {
                return await _context.OrderItems.AnyAsync(oi => oi.OrderItemID == orderItemID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while checking if the order item exists.", ex);
            }
        }

        public async Task DeleteOrderItemAsync(int orderItemID)
        {
            try
            {
                var orderItem = await _context.OrderItems.FindAsync(orderItemID);
                if (orderItem == null)
                {
                    throw new KeyNotFoundException($"OrderItem with ID {orderItemID} not found.");
                }

                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the order item.", ex);
            }
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemID)
        {
            try
            {
                // Define a cache key for the specific order item.
                string cacheKey = $"OrderItem_{orderItemID}";

                // Try to get the order item from the cache.
                if (!_cache.TryGetValue(cacheKey, out OrderItem cachedOrderItem))
                {
                    // If the order item is not found in the cache, retrieve it from the database.
                    var orderItem = await _context.OrderItems.FindAsync(orderItemID);

                    if (orderItem == null)
                    {
                        throw new KeyNotFoundException($"OrderItem with ID {orderItemID} not found.");
                    }

                    // Define cache options, such as expiration.
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache duration
                        SlidingExpiration = TimeSpan.FromMinutes(2) // Time to extend cache on each access
                    };

                    // Cache the order item for future use.
                    _cache.Set(cacheKey, orderItem, cacheOptions);

                    // Return the order item from the database.
                    return orderItem;
                }

                // Return the cached order item if it exists.
                return cachedOrderItem;
            }
            catch (Exception ex)
            {
                // Handle any exceptions and wrap them in an ApplicationException.
                throw new ApplicationException("An error occurred while retrieving the order item by ID.", ex);
            }
        }


        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderID)
        {
            try
            {
                // Define a cache key for the specific order's items.
                string cacheKey = $"OrderItems_{orderID}";

                // Try to get the order items from the cache.
                if (!_cache.TryGetValue(cacheKey, out IEnumerable<OrderItem> cachedOrderItems))
                {
                    // If the order items are not found in the cache, retrieve them from the database.
                    var orderItems = await _context.OrderItems
                        .Where(oi => oi.OrderID == orderID)
                        .ToListAsync();

                    if (orderItems == null)
                    {
                        throw new KeyNotFoundException($"OrderItem with ID {orderID} not found.");
                    }

                    // If no items are found, you might want to handle this case differently, e.g., return an empty list.

                    // Define cache options, such as expiration.
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache duration
                        SlidingExpiration = TimeSpan.FromMinutes(2) // Time to extend cache on each access
                    };

                    // Cache the order items for future use.
                    _cache.Set(cacheKey, orderItems, cacheOptions);

                    // Return the order items from the database.
                    return orderItems;
                }

                // Return the cached order items if they exist.
                return cachedOrderItems;
            }
            catch (Exception ex)
            {
                // Handle any exceptions and wrap them in an ApplicationException.
                throw new ApplicationException("An error occurred while retrieving order items by order ID.", ex);
            }
        }


        //public async Task<IEnumerable<OrderItem>> GetPopularOrderItemsAsync()
        //{
        //    try
        //    {
        //        return await _context.OrderItems
        //            .GroupBy(oi => oi.MenuItemID)
        //            .OrderByDescending(g => g.Count())
        //            .Select(g => g.FirstOrDefault())
        //            .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException("An error occurred while retrieving popular order items.", ex);
        //    }
        //}

        public async Task<double> GetTotalPriceForOrderAsync(int orderID)
        {
            try
            {
                var totalPrice = await _context.OrderItems
                    .Where(oi => oi.OrderID == orderID)
                    .SumAsync(oi => oi.Quantity * oi.MenuItem.Price); // Assuming you have a navigation property for MenuItem
                return totalPrice;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while calculating the total price for the order.", ex);
            }
        }

        public async Task UpdateOrderItemDetailsAsync(int orderItemID, OrderItem updatedOrderItem)
        {
            try
            {
                var existingOrderItem = await _context.OrderItems.FindAsync(orderItemID);
                if (existingOrderItem == null)
                {
                    throw new KeyNotFoundException($"OrderItem with ID {orderItemID} not found.");
                }

                existingOrderItem.MenuItemID = updatedOrderItem.MenuItemID;
                existingOrderItem.Quantity = updatedOrderItem.Quantity;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the order item details.", ex);
            }
        }

        public async Task UpdateOrderItemQuantityAsync(int orderItemID, int newQuantity)
        {
            try
            {
                var existingOrderItem = await _context.OrderItems.FindAsync(orderItemID);
                if (existingOrderItem == null)
                {
                    throw new KeyNotFoundException($"OrderItem with ID {orderItemID} not found.");
                }

                existingOrderItem.Quantity = newQuantity;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the order item quantity.", ex);
            }
        }
    }
}
