using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Src.IServices;
using restaurant_backend.Models.DTOs.PaymentDTOS;
using Microsoft.Extensions.Caching.Memory;

namespace restaurant_backend.Src.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMemoryCache _cache;

        public PaymentService(RestaurantDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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
                // Define a cache key for storing and retrieving the payments.
                string cacheKey = "AllPayments";

                // Try to get the list of payments from the cache.
                if (!_cache.TryGetValue(cacheKey, out IEnumerable<Payment> cachedPayments))
                {
                    // If the cache does not contain the payments, retrieve them from the database.
                    var payments = await _context.Payments.ToListAsync();

                    // Define cache options such as expiration.
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache duration
                        SlidingExpiration = TimeSpan.FromMinutes(2) // Time to extend cache on each access
                    };

                    // Cache the payments for future use.
                    _cache.Set(cacheKey, payments, cacheOptions);

                    // Return the payments from the database.
                    return payments;
                }

                // Return the cached payments if they exist.
                return cachedPayments;
            }
            catch (Exception ex)
            {
                // Handle any exceptions and wrap them in an ApplicationException.
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
