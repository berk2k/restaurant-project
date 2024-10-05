using Microsoft.AspNetCore.Mvc;
using restaurant_backend.Models;
using restaurant_backend.Models.DTOs.PaymentDTOS;
using restaurant_backend.Src.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace restaurant_backend.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        protected APIResponse _response;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _response = new APIResponse();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentRequestDTO paymentDto)
        {
            if (paymentDto == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "Payment data is null.";
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            try
            {
                await _paymentService.ProcessPaymentAsync(paymentDto);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while processing the payment: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = payment;
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
                _response.ErrorMessage = "An error occurred while retrieving the payment: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = payments;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while retrieving all payments: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }

        

        [HttpPost("refund/{id}")]
        public async Task<IActionResult> RefundPayment(int id)
        {
            try
            {
                var result = await _paymentService.RefundPaymentAsync(id);
                if (result)
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.OK;
                    _response.Result = "Payment refunded successfully.";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Payment not found.";
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "An error occurred while processing the refund: " + ex.Message;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }
    }
}
