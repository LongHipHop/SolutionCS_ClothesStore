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

        [HttpGet("daily-earnings")]
        public async Task<IActionResult> GetDailyEarnings()
        {
            var earnings = await _paymentService.GetTodayEarningsAsync();

            return Ok(new { date = DateTime.Now.Date, totalEarnings = earnings });
        }

        [HttpGet("earnings/weekly")]
        public async Task<IActionResult> GetWeeklyEarnings()
        {
            var total = await _paymentService.GetWeeklyEarningsAsync();
            return Ok(new { Week = "This week", Total = total });
        }

        [HttpGet("earnings/monthly/{year:int}/{month:int}")]
        public async Task<IActionResult> GetMonthlyEarnings(int year, int month)
        {
            var total = await _paymentService.GetMonthlyEarningsAsync(year, month);
            return Ok(new { Year = year, Month = month, Total = total });
        }

        [HttpGet("earnings/yearly/{year:int}")]
        public async Task<IActionResult> GetYearlyEarnings(int year)
        {
            var total = await _paymentService.GetYearlyEarningsAsync(year);
            return Ok(new { Year = year, Total = total });
        }

        [HttpGet("earnings/summary")]
        public async Task<IActionResult> GetAllEarningsSummary()
        {
            var summary = await _paymentService.GetAllEarningsSummaryAsync();
            return Ok(summary);
        }

        [HttpGet("revenue/daily")]
        public async Task<IActionResult> GetDailyRevenueAndCount()
        {
            var result = await _paymentService.GetDailyRevenueAndCountAsync();
            return Ok(result);
        }

        [HttpGet("revenue/range")]
        public async Task<IActionResult> GetRevenueByRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _paymentService.GetRevenueByDateRangeAsync(startDate, endDate);
            return Ok(result);
        }
    }
}
