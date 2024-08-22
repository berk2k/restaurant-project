using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_backend.Models.DTOs.OrderDTOS
{
    public class AddOrderItemRequestDTO
    {
        
        public int OrderID { get; set; }

        public int MenuItemID { get; set; }
        public int Quantity { get; set; }
    }
}
