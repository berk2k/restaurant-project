namespace restaurant_backend.Models.DTOs.PaymentDTOS
{
    public class CreatePaymentRequestDTO
    {
        public int OrderID { get; set; }
        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }

        public bool IsRefunded { get; set; }
    }
}
