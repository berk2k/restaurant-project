using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace restaurant_backend.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [ForeignKey("MenuItem")]
        public int MenuItemID { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}