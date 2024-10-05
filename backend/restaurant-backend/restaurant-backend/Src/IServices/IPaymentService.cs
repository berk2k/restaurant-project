using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.PaymentDTOS;

namespace restaurant_backend.Src.IServices
{
    public interface IPaymentService
    {
        Task ProcessPaymentAsync(CreatePaymentRequestDTO paymentRequestDTO);
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<bool> RefundPaymentAsync(int paymentId);

        
    }
}
