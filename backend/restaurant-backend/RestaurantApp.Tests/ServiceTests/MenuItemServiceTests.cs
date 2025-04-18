using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Models.DTOs.MenuDTOS;
using restaurant_backend.Src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Tests.ServiceTests
{
    public class MenuItemServiceTests
    {
        private RestaurantDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RestaurantDbContext(options);
        }

        [Fact]
        public async Task AddMenuItemAsync_Should_Add_Item_To_Database()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new MenuItemService(context);

            var newItemDto = new AddMenuItemRequestDTO
            {
                Name = "Burger",
                Description = "Delicious beef burger",
                Price = 12.99,
                ImageUrl = "https://example.com/burger.jpg",
                Category = "Fast Food"
            };

            // Act
            await service.AddMenuItemAsync(newItemDto);

            // Assert
            var items = await context.MenuItems.ToListAsync();
            Assert.Single(items);

            var item = items.First();
            Assert.Equal("Burger", item.Name);
            Assert.Equal("Delicious beef burger", item.Description);
            Assert.Equal(12.99, item.Price);
            Assert.Equal("Fast Food", item.Category);
        }

        [Fact]
        public async Task AddMenuItemAsync_Should_Throw_When_Name_Is_Empty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb1")
                .Options;

            using var context = new RestaurantDbContext(options);
            var service = new MenuItemService(context);

            var dto = new AddMenuItemRequestDTO
            {
                Name = "", 
                Description = "Test Description",
                Price = 10,
                ImageUrl = "https://example.com/image.jpg",
                Category = "Main"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.AddMenuItemAsync(dto));
            Assert.Equal("Menu item name cannot be empty.", ex.Message);
        }



    }
}
