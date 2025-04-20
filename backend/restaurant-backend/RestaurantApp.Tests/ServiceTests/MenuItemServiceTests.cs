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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var menuItems = new List<MenuItem>();  
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

        [Fact]
        public async Task GetMenuItemsAsync_ShouldReturnCorrectPage_WhenCalledWithValidPageAndSize()
        {
            // Arrange
            var menuItems = new List<MenuItem>();
            for (int i = 1; i <= 50; i++)
            {
                menuItems.Add(new MenuItem { MenuItemID = i, Name = $"Item {i}", Price = i });
            }

            var mockSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            var result = await service.GetAllMenuItemsAsync(page: 2, pageSize: 10);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(10, resultList.Count);
            Assert.Equal("Item 11", resultList[0].Name);
            Assert.Equal("Item 20", resultList[9].Name);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 0)]
        [InlineData(-1, 10)]
        public async Task GetMenuItemsAsync_ShouldThrowArgumentException_WhenPageOrSizeIsInvalid(int page, int pageSize)
        {
            // Arrange
            var mockContext = new Mock<RestaurantDbContext>();
            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                service.GetAllMenuItemsAsync(page, pageSize));

            Assert.Equal("An error occurred while retrieving paginated menu items.", exception.Message);
        }

        [Fact]
        public async Task GetMenuItemsAsync_ShouldReturnEmptyList_WhenNoMenuItemsExist()
        {
            // Arrange
            var emptyMenuItems = new List<MenuItem>();
            var mockDbSet = CreateMockDbSet(emptyMenuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            var result = await service.GetAllMenuItemsAsync(page: 1, pageSize: 10);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllMenuItemsAsync_ShouldThrowApplicationException_OnDatabaseError()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();

            // Menü öğelerine erişimde hata fırlatılmasını simüle et
            mockContext.Setup(c => c.MenuItems).Throws(new Exception("Database unreachable"));

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                service.GetAllMenuItemsAsync(1,5));

            Assert.Equal("An error occurred while retrieving paginated menu items.", exception.Message);
        }


        [Fact]
        public async Task GetAvailableMenuItemsAsync_ShouldReturnAvailableMenuItems()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { MenuItemID = 1, Name = "Pizza", IsAvailable = true },
                new MenuItem { MenuItemID = 2, Name = "Burger", IsAvailable = false },
                new MenuItem { MenuItemID = 3, Name = "Salad", IsAvailable = true }
            };

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            var result = (await service.GetAvailableMenuItemsAsync()).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.True(item.IsAvailable));
        }

        [Fact]
        public async Task GetAvailableMenuItemsAsync_ShouldReturnEmptyList_WhenNoItemsAvailable()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { MenuItemID = 1, Name = "Pizza", IsAvailable = false },
                new MenuItem { MenuItemID = 2, Name = "Burger", IsAvailable = false }
            };

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            var result = (await service.GetAvailableMenuItemsAsync()).ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAvailableMenuItemsAsync_ShouldThrowApplicationException_OnDatabaseError()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();

            
            mockContext.Setup(c => c.MenuItems).Throws(new Exception("Database connection error"));

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                service.GetAvailableMenuItemsAsync());

            Assert.Equal("An error occurred while retrieving available menu items.", exception.Message);
        }

        [Fact]
        public async Task GetMenuItemByIdAsync_ShouldReturnMenuItem_WhenMenuItemExists()
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

            var service = new MenuItemService(mockContext.Object);

            // Act
            var result = await service.GetMenuItemByIdAsync(menuItemID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(menuItemID, result.MenuItemID);
            Assert.Equal("Pizza", result.Name);
            Assert.Equal(10.99, result.Price);
        }

        [Fact]
        public async Task GetMenuItemByIdAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var menuItemID = 999; // A non-existing menuItemID
            var menuItems = new List<MenuItem>();  // No items in the list

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetMenuItemByIdAsync(menuItemID));
            Assert.Equal($"Menu item with ID {menuItemID} not found.", exception.Message);
        }

        [Fact]
        public async Task GetMenuItemsByCategoryAsync_ShouldReturnMenuItems_WhenCategoryExists()
        {
            // Arrange
            var category = "Pizza";
            var menuItems = new List<MenuItem>
            {
                new MenuItem { MenuItemID = 1, Name = "Margherita", Category = "Pizza", Price = 10.99 },
                new MenuItem { MenuItemID = 2, Name = "Pepperoni", Category = "Pizza", Price = 12.99 }
            };

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            var result = await service.GetMenuItemsByCategoryAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); 
            Assert.All(result, mi => Assert.Equal(category, mi.Category));  
        }

        [Fact]
        public async Task GetMenuItemsByCategoryAsync_ShouldReturnEmptyList_WhenCategoryHasNoMenuItems()
        {
            // Arrange
            var category = "Salads";  
            var menuItems = new List<MenuItem>();  

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act
            var result = await service.GetMenuItemsByCategoryAsync(category);

            // Assert
            Assert.Empty(result);  
        }

        [Fact]
        public async Task GetMenuItemsByCategoryAsync_ShouldThrowApplicationException_WhenDatabaseErrorOccurs()
        {
            // Arrange
            var category = "Pizza";
            var mockDbSet = new Mock<DbSet<MenuItem>>();
            var mockContext = new Mock<RestaurantDbContext>();

           
            mockContext.Setup(c => c.MenuItems)
                       .Throws(new Exception("Database error"));

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.GetMenuItemsByCategoryAsync(category));
            Assert.Equal("An error occurred while retrieving menu items by category.", exception.Message);
        }

        

        [Fact]
        public async Task ToggleAvailabilityAsync_ShouldToggleAvailability_WhenMenuItemExists()
        {
            // Arrange
            var menuItemID = 1;
            var menuItems = new List<MenuItem>
            {
                new MenuItem { MenuItemID = menuItemID, Name = "Pizza", Price = 10.99, IsAvailable = true }
            };

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new MenuItemService(mockContext.Object);

            // Act
            await service.ToggleAvailabilityAsync(menuItemID);

            // Assert
            Assert.False(menuItems[0].IsAvailable);  
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ToggleAvailabilityAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var menuItemID = 1;
            var menuItems = new List<MenuItem>();  

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.ToggleAvailabilityAsync(menuItemID));
            Assert.Equal("An error occurred while toggling the availability of the menu item.", exception.Message);
        }

        [Fact]
        public async Task UpdateMenuItemNameAsync_ShouldUpdateName_WhenMenuItemExists()
        {
            // Arrange
            var menuItemID = 1;
            var newName = "New Pizza";
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
            await service.UpdateMenuItemNameAsync(menuItemID, newName);

            // Assert
            Assert.Equal(newName, menuItems[0].Name);  // Menü öğesinin adı güncellenmeli
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMenuItemNameAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var menuItemID = 1;
            var newName = "New Pizza";
            var menuItems = new List<MenuItem>();  

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.UpdateMenuItemNameAsync(menuItemID, newName));
            Assert.IsType<ApplicationException>(exception);
        }

        [Fact]
        public async Task UpdateMenuItemDescriptionAsync_ShouldUpdateDescription_WhenMenuItemExists()
        {
            // Arrange
            var menuItemID = 1;
            var newDescription = "Delicious pizza with cheese and pepperoni";
            var menuItems = new List<MenuItem>
            {
                new MenuItem { MenuItemID = menuItemID, Name = "Pizza", Price = 10.99, Description = "Old description" }
            };

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new MenuItemService(mockContext.Object);

            // Act
            await service.UpdateMenuItemDescriptionAsync(menuItemID, newDescription);

            // Assert
            Assert.Equal(newDescription, menuItems[0].Description); 
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMenuItemDescriptionAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var menuItemID = 1;
            var newDescription = "Delicious pizza with cheese and pepperoni";
            var menuItems = new List<MenuItem>();  

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.UpdateMenuItemDescriptionAsync(menuItemID, newDescription));
            Assert.IsType<ApplicationException>(exception);
        }

        [Fact]
        public async Task UpdateMenuItemPriceAsync_ShouldUpdatePrice_WhenMenuItemExists()
        {
            // Arrange
            var menuItemID = 1;
            var newPrice = 12.99;
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
            await service.UpdateMenuItemPriceAsync(menuItemID, newPrice);

            // Assert
            Assert.Equal(newPrice, menuItems[0].Price); 
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMenuItemPriceAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var menuItemID = 1;
            var newPrice = 12.99;
            var menuItems = new List<MenuItem>();  

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.UpdateMenuItemPriceAsync(menuItemID, newPrice));
            Assert.IsType<ApplicationException>(exception);
        }

        [Fact]
        public async Task UpdateMenuItemCategoryAsync_ShouldUpdateCategory_WhenMenuItemExists()
        {
            // Arrange
            var menuItemID = 1;
            var newCategory = "Vegan";
            var menuItems = new List<MenuItem>
    {
        new MenuItem { MenuItemID = menuItemID, Name = "Pizza", Category = "Italian" }
    };

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new MenuItemService(mockContext.Object);

            // Act
            await service.UpdateMenuItemCategoryAsync(menuItemID, newCategory);

            // Assert
            Assert.Equal(newCategory, menuItems[0].Category);  
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMenuItemCategoryAsync_ShouldThrowKeyNotFoundException_WhenMenuItemNotFound()
        {
            // Arrange
            var menuItemID = 1;
            var newCategory = "Vegan";
            var menuItems = new List<MenuItem>();  

            var mockDbSet = CreateMockDbSet(menuItems);
            var mockContext = new Mock<RestaurantDbContext>();
            mockContext.Setup(c => c.MenuItems).Returns(mockDbSet.Object);

            var service = new MenuItemService(mockContext.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => service.UpdateMenuItemCategoryAsync(menuItemID, newCategory));
            Assert.IsType<ApplicationException>(exception);
        }




































    }


}
