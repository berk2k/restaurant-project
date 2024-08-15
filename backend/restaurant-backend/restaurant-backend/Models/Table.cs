using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace restaurant_backend.Models
{
    public class Table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableID { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }

        public required string QrCode { get; set; }
    }
}
