using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Src.Services;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs;
using NUnit.Framework.Interfaces;


namespace restaurant_tests.ServiceTests
{
    [TestFixture]
    public class TableServiceTests
    {
        private Mock<RestaurantDbContext> _mockContext;
        private Mock<DbSet<Table>> _mockTableSet;
        private TableService _tableService;


        [SetUp]
        public void SetUp()
        {
            // Mock DbSet
            _mockTableSet = new Mock<DbSet<Table>>();

            // Mock DbContext
            _mockContext = new Mock<RestaurantDbContext>();
            _mockContext.Setup(m => m.Tables).Returns(_mockTableSet.Object);

            // Initialize TableService with mocked context
            _tableService = new TableService(_mockContext.Object);
        }

        [Test]
        public async Task AddTableAsync_Should_AddTableToDatabase()
        {
            // Arrange
            var addTableRequest = new AddTableRequestDTO
            {
                TableNumber = 5,
                Capacity = 4
            };

            // Act
            await _tableService.AddTableAsync(addTableRequest);

            // Assert
            _mockTableSet.Verify(m => m.AddAsync(It.IsAny<Table>(), default), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        
    }
}
