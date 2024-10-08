﻿using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.TableDTOS;

namespace restaurant_backend.Src.IServices
{
    public interface ITableService
    {

        Task<IEnumerable<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(int tableId);
        Task AddTableAsync(AddTableRequestDTO dto);
        Task UpdateTableAsync(int tableNumber, int newCapacity, bool isAvailable);
        Task DeleteTableAsync(int tableNumber);
        Task<string> GetTableStatusAsync(int tableNumber);
        Task<bool> ReserveTableAsync(int tableNumber);
        Task<bool> ReleaseTableAsync(int tableNumber);
        Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableNumber);
        Task<Order> GetCurrentOrderForTableAsync(int tableNumber);
        Task<bool> CheckTableAvailabilityAsync(int tableNumber);

        Task AddQrCodeToTableAsync(int tableNumber);


    }
}
