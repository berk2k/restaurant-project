using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurant_backend.Models.DTOs.OrderDTOS;
using restaurant_backend.Models;
using restaurant_backend.Src.IServices;
using Microsoft.EntityFrameworkCore;

namespace restaurant_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        protected APIResponse _response;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
            _response = new APIResponse();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] AddOrderItemRequestDTO dto)
        {
            try
            {
                

                await _orderItemService.AddOrderItemAsync(dto);
                _response.IsSuccess = true;
                _response.ErrorMessage = "Order item added successfully.";
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
              
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                
                _response.IsSuccess = false;
                _response.ErrorMessage = "An unexpected error occurred.";
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpGet("by-item/{id:int}")]
        public async Task<IActionResult> GetOrderItemByItemId(int id)
        {
            try
            {
                var orderItem = await _orderItemService.GetOrderItemsByItemIdAsync(id);
                if (orderItem == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Order item not found.";
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = orderItem;
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            try
            {
                var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
                if (orderItem == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Order item not found.";
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = orderItem;
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("by-order/{orderId:int}")]
        public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
        {
            try
            {
                var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);
                _response.IsSuccess = true;
                _response.Result = orderItems;
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        //[HttpGet("popular")]
        //public async Task<IActionResult> GetPopularOrderItems()
        //{
        //    try
        //    {
        //        var popularItems = await _orderItemService.GetPopularOrderItemsAsync();
        //        _response.IsSuccess = true;
        //        _response.Result = popularItems;
        //        return Ok(_response);
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessage = ex.Message;
        //        return BadRequest(_response);
        //    }
        //}

        [HttpGet("total-price/{orderId:int}")]
        public async Task<IActionResult> GetTotalPriceForOrder(int orderId)
        {
            try
            {
                var totalPrice = await _orderItemService.GetTotalPriceForOrderAsync(orderId);
                _response.IsSuccess = true;
                _response.Result = totalPrice;
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPut("update-details/{id:int}")]
        public async Task<IActionResult> UpdateOrderItemDetails(int id, [FromBody] OrderItem updatedOrderItem)
        {
            try
            {
                await _orderItemService.UpdateOrderItemDetailsAsync(id, updatedOrderItem);
                _response.IsSuccess = true;
                _response.ErrorMessage = "Order item details updated successfully.";
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPut("update-quantity/{id:int}")]
        public async Task<IActionResult> UpdateOrderItemQuantity(int id, [FromBody] UpdateQuantityRequestDTO dto)
        {
            try
            {
                await _orderItemService.UpdateOrderItemQuantityAsync(id, dto.newQuantity);
                _response.IsSuccess = true;
                _response.ErrorMessage = "Order item quantity updated successfully.";
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            try
            {
                await _orderItemService.DeleteOrderItemAsync(id);
                _response.IsSuccess = true;
                _response.ErrorMessage = "Order item deleted successfully.";
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("exists/{id:int}")]
        public async Task<IActionResult> CheckIfOrderItemExists(int id)
        {
            try
            {
                var exists = await _orderItemService.CheckIfOrderItemExistsAsync(id);
                _response.IsSuccess = true;
                _response.Result = exists;
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrderItems()
        {
            try
            {
                var orderItems = await _orderItemService.GetAllOrderItemsAsync();
                _response.IsSuccess = true;
                _response.Result = orderItems;
                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpDelete("clear-basket")]
        public IActionResult ClearBasket()
        {
            

            try
            {
                _orderItemService.ClearBasketAsync();
                _response.IsSuccess = true;
                _response.Result = "Basket cleared succesfully";
                return Ok(_response);
            }
            catch(ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return BadRequest(_response);
            }
        }

    }
}
