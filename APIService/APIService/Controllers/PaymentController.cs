using APIService.HttpResponse;
using APIService.Models;
using APIService.Models.DTOs;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCUDTO payments)
        {
            if (payments == null)
            {
                var errorResponse = new APIResponse<object>
                {
                    Code = "1003",
                    Result = null
                };
                return Ok(errorResponse);
            }

            payments.PaymentDate = DateTime.Now;
            var codeResult = await _paymentService.CreatePaymentAsync(payments);

            string code = $"100{codeResult.Item2}";

            var response = new APIResponse<PaymentCUDTO>
            {
                Code = code,
                Result = codeResult.Item1
            };
            return Ok(response);
        }
    }
}
