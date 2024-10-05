using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace restaurant_backend.Models
{
    public class PaymentOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentOrderID { get; set; }

        [ForeignKey("Payment")]
        public int PaymentID { get; set; }
        public Payment Payment { get; set; } // Navigation property to Payment

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Order Order { get; set; } // Navigation property to Order
    }
}
