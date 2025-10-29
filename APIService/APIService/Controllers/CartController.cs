using APIService.HttpResponse;
using APIService.Models.DTOs;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO dto)
        {

            var result = await _cartService.AddToCartAsync(dto.AccountId, dto.ProductId, dto.ColorId, dto.SizeId, dto.Quantity);

            return result switch
            {
                0 => Ok(new APIResponse<string> { Code = "1000", Result = "Added to cart successfully!" }),
                2 => BadRequest(new APIResponse<string> { Code = "1002", Result = "Product variant not found!"}),
                _ => StatusCode(500, new APIResponse<string> { Code = "1001", Result = "Server error!"})
            };
        }

        [HttpGet("GetCart/{accountId}")]
        public async Task<IActionResult> GetCart(int accountId)
        {
            var cart = await _cartService.GetCartByAccountIdAsync(accountId);

            if(cart == null)
            {
                return NotFound((new APIResponse<string> { Code = "1002", Result = "Cart not found!"}));
            }
            return Ok(new APIResponse<object> { Code = "1000", Result = cart });
        }
    }
}
