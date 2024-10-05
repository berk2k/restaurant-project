using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace restaurant_backend.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public double AmountPaid { get; set; }

        public bool IsRefunded { get; set; }

        //public Order Order { get; set; }
    }
}