
using Microsoft.EntityFrameworkCore;
using QRCoder;
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

        public async Task AddQrCodeToTableAsync(int tableNumber)
        {
            try
            {
                
                string baseUrl = "https://www.example.com/table/";
                string qrCodeContent = $"{baseUrl}{tableNumber}"; // Örneğin: https://www.example.com/table/5

                
                string qrCodeBase64 = GenerateQRCodeBase64(qrCodeContent);

                
                if (string.IsNullOrEmpty(qrCodeBase64))
                {
                    throw new InvalidOperationException("QR kodu oluşturulamadı. Base64 verisi null veya boş.");
                }


                // Tabloyu bul veya oluştur
                Table table = await _context.Tables
                    .FirstOrDefaultAsync(t => t.TableNumber == tableNumber);

                if (table == null)
                {
                    
                    table = new Table
                    {
                        TableNumber = tableNumber,
                        QrCode = qrCodeBase64
                    };
                    _context.Tables.Add(table);
                }
                else
                {
                    
                    table.QrCode = qrCodeBase64;
                    _context.Tables.Update(table);
                }

                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                
            }
        }

        public async Task AddTableAsync(AddTableRequestDTO dto)
        {


            var newTable = new Table
            {
                TableNumber = dto.TableNumber,
                Capacity = dto.Capacity,
                QrCode = GenerateQRCodeBase64("https://www.example.com/table/")
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

        public async Task DeleteTableAsync(int tableNumber)
        {
            try
            {
                // Table'ı tableNumber ile bul
                var table = await _context.Tables
                    .Where(t => t.TableNumber == tableNumber)
                    .FirstOrDefaultAsync();

                if (table == null)
                {
                    // Tablo bulunamazsa özel bir istisna fırlatın
                    throw new InvalidOperationException($"Table with number {tableNumber} not found.");
                }

                // Tabloyu sil
                _context.Tables.Remove(table);

                // Değişiklikleri veritabanına kaydet
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Hata yönetimi: uygun bir şekilde loglama veya özel istisna fırlatma
                throw new ApplicationException("An error occurred while deleting the table.", ex);
            }
        }


        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            try
            {
                
                var tables = await _context.Tables.ToListAsync();

                return tables;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving tables.", ex);
            }
        }


        public async Task<Order> GetCurrentOrderForTableAsync(int tableNumber)
        {
            try
            {
                // Geçerli siparişi bulmak için tablodan siparişi getir
                var currentOrder = await _context.Orders
                    .Where(o => o.TableNumber == tableNumber && o.OrderStatus == "Pending") // "Pending" durumu örnektir, durumu kendi uygulamanıza göre uyarlayın
                    .FirstOrDefaultAsync();

                if (currentOrder == null)
                {
                    // Geçerli sipariş bulunamazsa null döndür
                    return null;
                }

                return currentOrder;
            }
            catch (Exception ex)
            {
                // Hata yönetimi: uygun bir şekilde loglama veya özel istisna fırlatma
                throw new ApplicationException("An error occurred while retrieving the current order.", ex);
            }
        }


        public async Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableNumber)
        {
            try
            {
                // Belirli bir masa numarasına sahip tüm siparişleri asenkron olarak al
                var orders = await _context.Orders
                    .Where(o => o.TableNumber == tableNumber)
                    .ToListAsync();

                return orders;
            }
            catch (Exception ex)
            {
                // Hata yönetimi: uygun bir şekilde loglama veya özel istisna fırlatma
                throw new ApplicationException("An error occurred while retrieving orders for the table.", ex);
            }
        }


        public Task<Table> GetTableByIdAsync(int tableId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetTableStatusAsync(int tableNumber)
        {
            try
            {
                var table = await _context.Tables
                    .Where(t => t.TableNumber == tableNumber)
                    .FirstOrDefaultAsync();

                if (table == null)
                {
                    throw new InvalidOperationException($"Table with number {tableNumber} not found.");
                }

                return table.IsAvailable ? "Available" : "Occupied"; 
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the table status.", ex);
            }
        }


        public async Task<bool> ReleaseTableAsync(int tableNumber)
        {
            try
            {
                var table = await _context.Tables
                    .Where(t => t.TableNumber == tableNumber)
                    .FirstOrDefaultAsync();

                if (table == null)
                {
                    throw new InvalidOperationException($"Table with number {tableNumber} not found.");
                }

                if (table.IsAvailable)
                {
                    return false; 
                }

                table.IsAvailable = true; 
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while releasing the table.", ex);
            }
        }


        public async Task<bool> ReserveTableAsync(int tableNumber)
        {
            try
            {
                var table = await _context.Tables
                    .Where(t => t.TableNumber == tableNumber)
                    .FirstOrDefaultAsync();

                if (table == null)
                {
                    throw new InvalidOperationException($"Table with number {tableNumber} not found.");
                }

                if (!table.IsAvailable)
                {
                    return false; 
                }

                table.IsAvailable = false; 
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while reserving the table.", ex);
            }
        }


        public async Task UpdateTableAsync(int tableNumber, int newCapacity, bool isAvailable)
        {
            try
            {
                var table = await _context.Tables
                    .Where(t => t.TableNumber == tableNumber)
                    .FirstOrDefaultAsync();

                if (table == null)
                {
                    throw new InvalidOperationException($"Table with number {tableNumber} not found.");
                }

                // Tabloyu güncelle
                table.Capacity = newCapacity;
                table.IsAvailable = isAvailable;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the table.", ex);
            }
        }


        private string GenerateQRCodeBase64(string content)
        {
            // QR kodu oluşturma işlemini burada yapıyoruz
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                using (BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData))
                {
                    byte[] qrCodeImage = qrCode.GetGraphic(20);
                    return Convert.ToBase64String(qrCodeImage);
                }
            }
        }
    }
}
