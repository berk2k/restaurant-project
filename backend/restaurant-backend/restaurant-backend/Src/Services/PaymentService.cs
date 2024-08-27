using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Src.IServices;
using restaurant_backend.Models.DTOs.PaymentDTOS;

namespace restaurant_backend.Src.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly RestaurantDbContext _context;

        public PaymentService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task ProcessPaymentAsync(CreatePaymentRequestDTO paymentDto)
        {
            try
            {
                var newPayment = new Payment
                {
                    OrderID = paymentDto.OrderID,
                    PaymentTime = paymentDto.PaymentTime,
                    PaymentMethod = paymentDto.PaymentMethod,
                    AmountPaid = paymentDto.AmountPaid,
                    IsRefunded = false
                };

                await _context.Payments.AddAsync(newPayment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the payment.", ex);
            }
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            try
            {
                
                return await _context.Payments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all payments.", ex);
            }
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(paymentId);

                if (payment == null)
                {
                    throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
                }

                return payment;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the payment by ID.", ex);
            }
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.OrderID == orderId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving payments for the order.", ex);
            }
        }

        public async Task<decimal> GetTotalPaymentsForOrderAsync(int orderId)
        {
            try
            {
                return await _context.Payments
                    .Where(p => p.OrderID == orderId)
                    .SumAsync(p => p.AmountPaid);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while calculating the total payments for the order.", ex);
            }
        }

        

        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(paymentId);

                if (payment == null)
                {
                    throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
                }

                
                payment.IsRefunded = true;

                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while processing the refund.", ex);
            }
        }
    }
}
