namespace restaurant_backend.Models.DTOs.PaymentDTOS
{
    public class CreatePaymentRequestDTO
    {
        
        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public double AmountPaid { get; set; }

        public bool IsRefunded { get; set; }

        public List<int> OrderIDs { get; set; }
    }
}
