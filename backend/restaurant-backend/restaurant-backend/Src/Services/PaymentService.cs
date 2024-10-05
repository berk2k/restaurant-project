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
                // Create a new payment record
                var newPayment = new Payment
                {
                    PaymentTime = paymentDto.PaymentTime,
                    PaymentMethod = paymentDto.PaymentMethod,
                    AmountPaid = paymentDto.AmountPaid,
                    IsRefunded = paymentDto.IsRefunded // Use the IsRefunded value from the DTO
                };

                // Add the payment to the context
                await _context.Payments.AddAsync(newPayment);
                await _context.SaveChangesAsync();

                // Now, relate the payment to each order ID in the list
                foreach (var orderID in paymentDto.OrderIDs)
                {
                    var paymentOrder = new PaymentOrder
                    {
                        PaymentID = newPayment.PaymentID, // Get the newly created payment ID
                        OrderID = orderID // Use the OrderID directly
                    };

                    await _context.PaymentOrders.AddAsync(paymentOrder);
                }

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
                // Retrieve all payments from the database without caching
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
