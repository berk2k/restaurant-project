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
                return await _context.OrderItems
                    .Where(oi => oi.OrderID == orderID)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
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
