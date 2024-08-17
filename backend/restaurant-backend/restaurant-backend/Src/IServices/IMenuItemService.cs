using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.MenuDTOS;

namespace restaurant_backend.Src.IServices
{
    public interface IMenuItemService
    {
        public Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
        public Task<MenuItem> GetMenuItemByIdAsync(int menuItemID);

        public Task AddMenuItemAsync(AddMenuItemRequestDTO dto);

        public Task UpdateMenuItemNameAsync(int menuItemID, string newName);
        public Task UpdateMenuItemDescriptionAsync(int menuItemID, string newDescription);
        public Task UpdateMenuItemPriceAsync(int menuItemID, double newPrice);
        public Task UpdateMenuItemCategoryAsync(int menuItemID, string newCategory);

        public Task DeleteMenuItemAsync(int menuItemID);

        public Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(string category);

        public Task ToggleAvailabilityAsync(int menuItemID);

        public Task<IEnumerable<MenuItem>> GetAvailableMenuItemsAsync();









    }
}
