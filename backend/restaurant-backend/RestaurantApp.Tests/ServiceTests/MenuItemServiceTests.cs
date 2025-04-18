using Microsoft.EntityFrameworkCore;
using Moq;
using restaurant_backend.Context;
using restaurant_backend.Models;
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
        public async Task AddMenuItemAsync_ShouldAddMenuItem_WhenValidDataIsProvided()
        {
            // Arrange
            var dto = new AddMenuItemRequestDTO
            {
                Name = "Pizza",
                Price = 9.99,
                Category = "Main Course",
                Description = "Delicious pizza"
            };
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(m => m.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            await service.AddMenuItemAsync(dto);

            // Assert
            mockDbSet.Verify(m => m.AddAsync(It.IsAny<MenuItem>(), It.IsAny<CancellationToken>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }


        [Fact]
        public async Task AddMenuItemAsync_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            // Arrange
            var dto = new AddMenuItemRequestDTO
            {
                Name = "",
                Price = 9.99,
                Category = "Main Course",
                Description = "Delicious pizza"
            };
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(m => m.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddMenuItemAsync(dto));
            Assert.Equal("Menu item name cannot be empty.", exception.Message);
        }


        [Fact]
        public async Task AddMenuItemAsync_ShouldThrowArgumentException_WhenPriceIsNegative()
        {
            // Arrange
            var dto = new AddMenuItemRequestDTO
            {
                Name = "Pizza",
                Price = -5,
                Category = "Main Course",
                Description = "Delicious pizza"
            };
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(m => m.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddMenuItemAsync(dto));
            Assert.Equal("Menu item price cannot be negative.", exception.Message);
        }


        [Fact]
        public async Task AddMenuItemAsync_ShouldThrowArgumentException_WhenCategoryIsEmpty()
        {
            // Arrange
            var dto = new AddMenuItemRequestDTO
            {
                Name = "Pizza",
                Price = 9.99,
                Category = "",
                Description = "Delicious pizza"
            };
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(m => m.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddMenuItemAsync(dto));
            Assert.Equal("Menu item category cannot be empty.", exception.Message);
        }

        [Fact]
        public async Task AddMenuItemAsync_ShouldThrowArgumentException_WhenNullDataIsProvided()
        {
            // Arrange
            var dto = new AddMenuItemRequestDTO
            {
                Name = null,
                Price = 0,
                Category = null,
                Description = null
            };
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(m => m.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddMenuItemAsync(dto));
            Assert.Equal("Menu item name cannot be empty.", exception.Message);
        }

        [Fact]
        public async Task AddMenuItemAsync_ShouldPersistMenuItemInDatabase_WhenValidDataIsProvided()
        {
            // Arrange
            var dto = new AddMenuItemRequestDTO
            {
                Name = "Burger",
                Price = 12.99,
                Category = "Main Course",
                Description = "Tasty burger"
            };
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(m => m.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            await service.AddMenuItemAsync(dto);

            // Assert
            mockDbSet.Verify(m => m.AddAsync(It.IsAny<MenuItem>(), It.IsAny<CancellationToken>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task AddMenuItemAsync_ShouldThrowException_WhenDatabaseFailsToPersist()
        {
            // Arrange
            var dto = new AddMenuItemRequestDTO
            {
                Name = "Burger",
                Price = 12.99,
                Category = "Main Course",
                Description = "Tasty burger"
            };
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(m => m.MenuItems).Returns(mockDbSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new DbUpdateException());

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.AddMenuItemAsync(dto));
            Assert.Equal("An error occurred while adding the menu item.", exception.Message);
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            // Mock the Remove method
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(entity => data.Remove(entity));

            return mockSet;
        }





        [Fact]
        public async Task DeleteMenuItemAsync_ShouldDeleteMenuItem_WhenMenuItemExists()
        {
            // Arrange
            var menuItemID = 1;
            var menuItems = new List<MenuItem>
            {   
                new MenuItem { MenuItemID = menuItemID, Name = "Pizza", Price = 10.99 }
            };

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new MenuItemService(mockContext.Object);

            // Act
            await service.DeleteMenuItemAsync(menuItemID);

            // Assert
            Assert.Empty(menuItems); 
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteMenuItemAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var menuItemID = 1;
            var menuItems = new List<MenuItem>();  // Menü öğesi yok
            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteMenuItemAsync(menuItemID));

            // Assert only the exception type without worrying about the exact message
            Assert.IsType<KeyNotFoundException>(exception);
        }

        [Fact]
        public async Task DeleteMenuItemAsync_ShouldThrowApplicationException_WhenDatabaseFails()
        {
            // Arrange
            var menuItemID = 1;
            var menuItems = new List<MenuItem>
            {
                new MenuItem { MenuItemID = menuItemID, Name = "Pizza", Price = 10.99 }
            };
            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new DbUpdateException());

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteMenuItemAsync(menuItemID));

            // Assert only the exception type
            Assert.IsType<ApplicationException>(exception);
        }









    }


}
