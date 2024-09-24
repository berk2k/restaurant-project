using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace restaurant_backend.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [ForeignKey("Table")]
        public int TableID { get; set; }

        public int TableNumber { get; set; }
        public DateTime OrderTime { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }

        public string ItemName { get; set; }

        public int Quantity { get; set; }

        
       
    }
}