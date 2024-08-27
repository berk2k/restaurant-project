using Microsoft.AspNetCore.Mvc;
using restaurant_backend.Models;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        protected APIResponse _response;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            _response = new APIResponse();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                await _orderService.CreateOrderAsync(order);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while creating the order: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                _response.Result = order;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while retrieving the order by ID: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                _response.Result = orders;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while retrieving all orders: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("table/{tableNumber}")]
        public async Task<IActionResult> GetOrdersByTableNumber(int tableNumber)
        {
            try
            {
                var orders = await _orderService.GetOrdersByTableNumberAsync(tableNumber);
                _response.Result = orders;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while retrieving orders by table number: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An unexpected error occurred: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{orderId}/price")]
        public async Task<IActionResult> UpdateTotalPrice(int orderId, [FromBody] double newTotalPrice)
        {
            try
            {
                await _orderService.UpdateTotalPriceAsync(orderId, newTotalPrice);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An unexpected error occurred: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var success = await _orderService.CancelOrderAsync(orderId);
                if (success)
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                    return NoContent();
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.ErrorMessage = "The order could not be canceled.";
                    return BadRequest(_response);
                }
            }
            catch (KeyNotFoundException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while canceling the order: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("mostRecent/{tableNumber}")]
        public async Task<IActionResult> GetMostRecentOrderForTable(int tableNumber)
        {
            try
            {
                var order = await _orderService.GetMostRecentOrderForTableAsync(tableNumber);
                _response.Result = order;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while retrieving the most recent order for the table: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOrders()
        {
            try
            {
                var orders = await _orderService.GetPendingOrdersAsync();
                _response.Result = orders;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while retrieving pending orders: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }
    }
}
