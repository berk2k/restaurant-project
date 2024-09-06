using Microsoft.EntityFrameworkCore;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.OrderDTOS;

namespace restaurant_backend.Src.IServices
{
    public interface IOrderItemService
    {
        Task AddOrderItemAsync(AddOrderItemRequestDTO dto);

        Task DeleteOrderItemAsync(int orderItemID);

        Task UpdateOrderItemQuantityAsync(int orderItemID, int newQuantity);

        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderID);

        Task<OrderItem> GetOrderItemByIdAsync(int orderItemID);

        Task<double> GetTotalPriceForOrderAsync(int orderID);

        Task<bool> CheckIfOrderItemExistsAsync(int orderItemID);

        Task UpdateOrderItemDetailsAsync(int orderItemID, OrderItem updatedOrderItem);
        Task<List<OrderItem>> GetAllOrderItemsAsync();

        Task<IEnumerable<OrderItem>> GetOrderItemsByItemIdAsync(int itemID);


        //Task<IEnumerable<OrderItem>> GetPopularOrderItemsAsync();
    }
}
