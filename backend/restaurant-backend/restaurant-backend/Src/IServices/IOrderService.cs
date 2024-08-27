using restaurant_backend.Models;

namespace restaurant_backend.Src.IServices
{
    public interface IOrderService
    {
        // Creates a new order
        Task CreateOrderAsync(Order newOrder);

        // Retrieves an order by its ID
        Task<Order> GetOrderByIdAsync(int orderId);

        // Retrieves all orders
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        // Retrieves orders by table number
        Task<IEnumerable<Order>> GetOrdersByTableNumberAsync(int tableNumber);

        // Updates the order status
        Task UpdateOrderStatusAsync(int orderId, string newStatus);

        // Updates the total price of the order
        Task UpdateTotalPriceAsync(int orderId, double newTotalPrice);

        // Cancels an order
        Task<bool> CancelOrderAsync(int orderId);

        // Retrieves the most recent order for a table
        Task<Order> GetMostRecentOrderForTableAsync(int tableNumber);


        // Retrieves pending orders
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
    }

}
