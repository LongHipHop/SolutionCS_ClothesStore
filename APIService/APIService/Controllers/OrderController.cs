using APIService.HttpResponse;
using APIService.Models.DTOs;
using APIService.Service.Implementations;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await _orderService.GetAll();

            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<OrderDTO>>
            {
                Code = code,
                Result = (IEnumerable<OrderDTO>)item.Item1
            };

            return Ok(response);
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var item = await _orderService.GetOrderById(id);

            string code = $"100{item.Item2}";

            var response = new APIResponse<OrderDTO>
            {
                Code = code,
                Result = item.Item1
            };

            return Ok(response);
        }

        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1003",
                    Result = null
                });
            }

            var codeResult = await _orderService.UpdateOrder(orderDTO);

            string code = $"100{codeResult}";

            var response = new APIResponse<OrderDTO>
            {
                Code = code,
                Result = orderDTO
            };

            return Ok(response);
        }
    }
}
