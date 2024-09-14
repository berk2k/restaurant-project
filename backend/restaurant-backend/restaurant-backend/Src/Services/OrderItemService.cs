using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.OrderDTOS;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Src.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly RestaurantDbContext _context;

        public OrderItemService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderItemAsync(AddOrderItemRequestDTO dto)
        {
            try
            {
                // Retrieve menu item details
                var menuItem = await GetMenuItemDetailsAsync(dto.MenuItemID);

                var orderItem = new OrderItem
                {
                    OrderID = 4, // Placeholder for OrderID, to be updated later
                    ItemName = menuItem.Name,  // Set from menu item details
                    MenuItemID = dto.MenuItemID,
                    ItemPrice = menuItem.Price, // Set from menu item details
                    Quantity = dto.Quantity,
                    MenuItem = menuItem,
                    ItemImg = menuItem.ImageUrl
                };

                await _context.OrderItems.AddAsync(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-related exceptions
                throw new ApplicationException("An error occurred while adding the order item to the database.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                throw new ApplicationException("An unexpected error occurred while adding the order item.", ex);
            }
        }

        public async Task<MenuItem> GetMenuItemDetailsAsync(int menuItemID)
        {
            try
            {
                var menuItem = await _context.MenuItems
                    .Where(m => m.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                if (menuItem == null)
                {
                    throw new ApplicationException("Menu item not found.");
                }

                return menuItem;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving menu item details.", ex);
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

        public async Task<List<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task ClearBasketAsync()
        {
            try
            {
                var orderItems = _context.OrderItems.ToList();

                if (orderItems.Count > 0)
                {
                    // Sepetteki öğeleri veritabanından sil
                    _context.OrderItems.RemoveRange(orderItems);
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured cleaning basket.", ex);
            }
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemID)
        {
            try
            {
                var orderItem = await _context.OrderItems.FindAsync(orderItemID);

                if (orderItem == null)
                {
                    throw new KeyNotFoundException($"OrderItem with ID {orderItemID} not found.");
                }

                return orderItem;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the order item by ID.", ex);
            }
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderID)
        {
            try
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderID == orderID)
                    .ToListAsync();

                if (orderItems == null)
                {
                    throw new KeyNotFoundException($"No order items found for order ID {orderID}.");
                }

                return orderItems;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving order items by order ID.", ex);
            }
        }

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

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByItemIdAsync(int itemID)
        {
            try
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.MenuItemID == itemID)
                    .ToListAsync();

                if (orderItems == null)
                {
                    throw new KeyNotFoundException($"No order items found for order ID {itemID}.");
                }

                return orderItems;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving order items by order ID.", ex);
            }
        }

        

        
    }
}
