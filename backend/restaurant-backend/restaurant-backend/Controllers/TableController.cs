using Microsoft.AspNetCore.Mvc;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.TableDTOS;
using restaurant_backend.Src.IServices;

namespace restaurant_backend.Controllers
{
    [Route("api/Table")]
    [ApiController]
    public class TableController : Controller
    {
        protected APIResponse _response;
        private ITableService _tableService;

        public TableController(ITableService service)
        {
            _tableService = service;
            _response = new APIResponse();
        }

        [HttpPost("AddTable")]
        public async Task<IActionResult> AddTable([FromBody] AddTableRequestDTO addTableRequest)
        {
            try
            {
                if (addTableRequest == null || !ModelState.IsValid)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Invalid table data.";
                    return BadRequest(_response);
                }

                // DTO'yu doğrudan service'e gönderiyoruz
                await _tableService.AddTableAsync(addTableRequest);

                _response.Result = addTableRequest;
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtAction(nameof(AddTable), new { id = addTableRequest.TableNumber }, _response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("CheckAvailability/{tableNumber}")]
        public async Task<IActionResult> CheckAvailability(int tableNumber)
        {
            try
            {
                bool isAvailable = await _tableService.CheckTableAvailabilityAsync(tableNumber);

                _response.Result = isAvailable;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("GetAllTables")]
        public async Task<IActionResult> GetAllTables()
        {
            try
            {
                var tables = await _tableService.GetAllTablesAsync();

                _response.Result = tables;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("GetCurrentOrder/{tableNumber}")]
        public async Task<IActionResult> GetCurrentOrder(int tableNumber)
        {
            try
            {
                var currentOrder = await _tableService.GetCurrentOrderForTableAsync(tableNumber);

                if (currentOrder == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "No current order found for this table.";
                    return NotFound(_response);
                }

                _response.Result = currentOrder;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("GetOrdersByTable/{tableNumber}")]
        public async Task<IActionResult> GetOrdersByTable(int tableNumber)
        {
            try
            {
                var orders = await _tableService.GetOrdersByTableAsync(tableNumber);

                if (orders == null || !orders.Any())
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "No orders found for this table.";
                    return NotFound(_response);
                }

                _response.Result = orders;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("GetTableById/{tableId}")]
        public async Task<IActionResult> GetTableById(int tableId)
        {
            try
            {
                var table = await _tableService.GetTableByIdAsync(tableId);

                _response.Result = table;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return NotFound(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }

        // Endpoint for GetTableStatusAsync
        [HttpGet("GetTableStatus/{tableNumber}")]
        public async Task<IActionResult> GetTableStatus(int tableNumber)
        {
            try
            {
                var status = await _tableService.GetTableStatusAsync(tableNumber);

                _response.Result = status;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (InvalidOperationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return NotFound(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }
        [HttpPost("ReleaseTable/{tableNumber}")]
        public async Task<IActionResult> ReleaseTable(int tableNumber)
        {
            try
            {
                var result = await _tableService.ReleaseTableAsync(tableNumber);

                if (!result)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Table is already available or not found.";
                    return BadRequest(_response);
                }

                _response.Result = "Table released successfully.";
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (InvalidOperationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return NotFound(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }

        // Endpoint for reserving a table
        [HttpPost("ReserveTable/{tableNumber}")]
        public async Task<IActionResult> ReserveTable(int tableNumber)
        {
            try
            {
                var result = await _tableService.ReserveTableAsync(tableNumber);

                if (!result)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Table is already reserved or not found.";
                    return BadRequest(_response);
                }

                _response.Result = "Table reserved successfully.";
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (InvalidOperationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message             ;
                return NotFound(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPut("UpdateTable/{tableNumber}")]
        public async Task<IActionResult> UpdateTable(int tableNumber, [FromBody] UpdateTableRequestDTO updateTableRequest)
        {
            try
            {
                // Call service method to update table
                await _tableService.UpdateTableAsync(tableNumber, updateTableRequest.NewCapacity, updateTableRequest.IsAvailable);

                _response.Result = "Table updated successfully.";
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (InvalidOperationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return NotFound(_response);
            }
            catch (ApplicationException ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
                return StatusCode(500, _response);
            }
        }


    }
}
