using Microsoft.EntityFrameworkCore;
using restaurant_backend.Models;

namespace restaurant_backend.Context
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext()
        {

        }
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentOrder> PaymentOrders { get; set; }

    }

   
}
