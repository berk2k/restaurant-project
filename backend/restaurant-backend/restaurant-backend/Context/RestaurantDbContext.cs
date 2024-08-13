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
        public DbSet<Table> Tables { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }

   
}
