namespace restaurant_backend.Models.DTOs.OrderDTOS
{
    public class AddOrderDTO
    {

        public int TableNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
    }
}
