using Microsoft.AspNetCore.Mvc;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.MenuDTOS;

using restaurant_backend.Src.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMenuItemService _menuService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _response = new APIResponse();
            _menuService = menuItemService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMenuItem([FromBody] AddMenuItemRequestDTO dto)
        {
            try
            {
                await _menuService.AddMenuItemAsync(dto);
                _response.ErrorMessage = "Menu item added successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpDelete("delete/{menuItemID}")]
        public async Task<IActionResult> DeleteMenuItem(int menuItemID)
        {
            try
            {
                await _menuService.DeleteMenuItemAsync(menuItemID);
                _response.ErrorMessage = "Menu item deleted successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableMenuItems()
        {
            try
            {
                var items = await _menuService.GetAvailableMenuItemsAsync();
                _response.Result = items;
                _response.ErrorMessage = "Available menu items retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{menuItemID}")]
        public async Task<IActionResult> GetMenuItemById(int menuItemID)
        {
            try
            {
                var item = await _menuService.GetMenuItemByIdAsync(menuItemID);
                if (item == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Menu item not found.";
                    return NotFound(_response);
                }

                _response.Result = item;
                _response.ErrorMessage = "Menu item retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetMenuItemsByCategory(string category)
        {
            try
            {
                var items = await _menuService.GetMenuItemsByCategoryAsync(category);
                _response.Result = items;
                _response.ErrorMessage = "Menu items by category retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPost("toggle/{menuItemID}")]
        public async Task<IActionResult> ToggleAvailability(int menuItemID)
        {
            try
            {
                await _menuService.ToggleAvailabilityAsync(menuItemID);
                _response.ErrorMessage = "Menu item availability toggled successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems()
        {
            try
            {
                var items = await _menuService.GetAllMenuItemsAsync();
                _response.Result = items;
                _response.ErrorMessage = "All menu items retrieved successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{menuItemID}/name")]
        public async Task<IActionResult> UpdateMenuItemName(int menuItemID, [FromBody] string newName)
        {
            try
            {
                await _menuService.UpdateMenuItemNameAsync(menuItemID, newName);
                _response.ErrorMessage = "Menu item name updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{menuItemID}/description")]
        public async Task<IActionResult> UpdateMenuItemDescription(int menuItemID, [FromBody] string newDescription)
        {
            try
            {
                await _menuService.UpdateMenuItemDescriptionAsync(menuItemID, newDescription);
                _response.ErrorMessage = "Menu item description updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{menuItemID}/price")]
        public async Task<IActionResult> UpdateMenuItemPrice(int menuItemID, [FromBody] double newPrice)
        {
            try
            {
                await _menuService.UpdateMenuItemPriceAsync(menuItemID, newPrice);
                _response.ErrorMessage = "Menu item price updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{menuItemID}/category")]
        public async Task<IActionResult> UpdateMenuItemCategory(int menuItemID, [FromBody] string newCategory)
        {
            try
            {
                await _menuService.UpdateMenuItemCategoryAsync(menuItemID, newCategory);
                _response.ErrorMessage = "Menu item category updated successfully.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = $"An error occurred: {ex.Message}";
                return StatusCode(500, _response);
            }
        }

    }
}
