using Microsoft.EntityFrameworkCore;
using restaurant_backend.Context;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.MenuDTOS;
using restaurant_backend.Models.DTOs.TableDTOS;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Src.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly RestaurantDbContext _context;
        public MenuItemService(RestaurantDbContext context) { _context = context; }


        public async Task AddMenuItemAsync(AddMenuItemRequestDTO dto)
        {
            var newItem = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                IsAvailable = true,
                Price = dto.Price,
                Category = dto.Category
            };


            await _context.MenuItems.AddAsync(newItem);


            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int menuItemID)
        {
            try
            {
                
                var menuItem = await _context.MenuItems
                    .Where(m => m.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                
                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"Menu item with ID {menuItemID} not found.");
                }

                
                _context.MenuItems.Remove(menuItem);

               
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while deleting the menu item.", ex);
            }
        }


        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
            try
            {
                
                var menuItems = await _context.MenuItems.ToListAsync();

                return menuItems;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving all menu items.", ex);
            }
        }


        public async Task<IEnumerable<MenuItem>> GetAvailableMenuItemsAsync()
        {
            try
            {
                
                var availableMenuItems = await _context.MenuItems
                    .Where(m => m.IsAvailable)
                    .ToListAsync();

                return availableMenuItems;
            }
            catch (Exception ex)
            {
               
                throw new ApplicationException("An error occurred while retrieving available menu items.", ex);
            }
        }


        public async Task<MenuItem> GetMenuItemByIdAsync(int menuItemID)
        {
            try
            {
                
                var menuItem = await _context.MenuItems
                    .Where(m => m.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                
                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"Menu item with ID {menuItemID} not found.");
                }

                return menuItem;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving the menu item by ID.", ex);
            }
        }


        public async Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(string category)
        {
            try
            {
                
                var menuItems = await _context.MenuItems
                    .Where(m => m.Category == category)
                    .ToListAsync();

                return menuItems;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving menu items by category.", ex);
            }
        }


        public async Task ToggleAvailabilityAsync(int menuItemID)
        {
            try
            {
                
                var menuItem = await _context.MenuItems
                    .Where(m => m.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                
                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"Menu item with ID {menuItemID} not found.");
                }

                
                menuItem.IsAvailable = !menuItem.IsAvailable;

                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while toggling the availability of the menu item.", ex);
            }
        }


        public async Task UpdateMenuItemNameAsync(int menuItemID, string newName)
        {
            try
            {
                var menuItem = await _context.MenuItems
                    .Where(mi => mi.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"Menu item with ID {menuItemID} not found.");
                }

                menuItem.Name = newName;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the menu item name.", ex);
            }
        }

        public async Task UpdateMenuItemDescriptionAsync(int menuItemID, string newDescription)
        {
            try
            {
                var menuItem = await _context.MenuItems
                    .Where(mi => mi.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"Menu item with ID {menuItemID} not found.");
                }

                menuItem.Description = newDescription;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the menu item description.", ex);
            }
        }

        public async Task UpdateMenuItemPriceAsync(int menuItemID, double newPrice)
        {
            try
            {
                var menuItem = await _context.MenuItems
                    .Where(mi => mi.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"Menu item with ID {menuItemID} not found.");
                }

                menuItem.Price = newPrice;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the menu item price.", ex);
            }
        }

        public async Task UpdateMenuItemCategoryAsync(int menuItemID, string newCategory)
        {
            try
            {
                var menuItem = await _context.MenuItems
                    .Where(mi => mi.MenuItemID == menuItemID)
                    .FirstOrDefaultAsync();

                if (menuItem == null)
                {
                    throw new KeyNotFoundException($"Menu item with ID {menuItemID} not found.");
                }

                menuItem.Category = newCategory;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the menu item category.", ex);
            }
        }

        

    }
}
