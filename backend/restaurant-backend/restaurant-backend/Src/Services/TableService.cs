
using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Src.Services
{
    public class TableService : ITableService
    {
        private readonly RestaurantDbContext _context;
        public TableService(RestaurantDbContext dbContext) {
            _context = dbContext;
        }

        public async Task AddTableAsync(AddTableRequestDTO dto)
        {

            
            var newTable = new Table
            {
                TableNumber = dto.TableNumber,
                Capacity = dto.Capacity
            };

            
            await _context.Tables.AddAsync(newTable);

            
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckTableAvailabilityAsync(int tableNumber)
        {
            try
            {
                
                var table = await _context.Tables
                    .Where(t => t.TableNumber == tableNumber)
                    .AsQueryable()
                    .FirstOrDefaultAsync();

                
                return table != null && table.IsAvailable;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while checking table availability.", ex);
            }
        }

        public Task DeleteTableAsync(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetCurrentOrderForTableAsync(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Table> GetTableByIdAsync(int tableId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTableStatusAsync(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReleaseTableAsync(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReserveTableAsync(int tableNumber)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTableAsync(int tableNumber)
        {
            throw new NotImplementedException();
        }
    }
}
