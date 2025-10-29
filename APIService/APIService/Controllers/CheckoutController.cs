using APIService.HttpResponse;
using APIService.Models;
using APIService.Models.DTOs;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using System.Linq;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CheckoutController(IPaymentService paymentService, ICartService cartService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpPost("ProceedToCheckout")]
        public async Task<IActionResult> ProceedToCheckout([FromBody] CheckoutDTO dto)
        {
            if(dto == null || dto.SelectedItems == null || !dto.SelectedItems.Any())
            {
                return BadRequest(new APIResponse<string> { Code = "1002", Result = "No items selected" });
            }

            var cart = await _cartService.GetCartByAccountIdAsync(dto.AccountId);
            if (cart == null || !cart.Items.Any())
            {
                return BadRequest(new APIResponse<string> { Code = "1004", Result = "Cart is empty!" });
            }

            var selectedProductIds = dto.SelectedItems.Select(x => x.ProductId).ToList();

            var selectedItems = cart.Items
                .Where(ci => selectedProductIds.Contains(ci.ProductId))
                .ToList();

            if (!selectedItems.Any())
                return BadRequest(new APIResponse<string> { Code = "1002", Result = "No items selected" } );


            double total = cart.Items.Sum(x => x.Price * x.Quantity);

            var (orderId, code) = await _orderService.CreateOrder(new OrderDTO
            {
                AccountId = dto.AccountId,
                OrderDate = DateTime.Now,
                TotalPrice = total,
                Status = "Pending",
                ShippingAddress = dto.ShippingAddress ?? "No address provided",
                Note = dto.Note ?? ""

            });

            if (code != 0)
            {
                return StatusCode(500, new APIResponse<string> { Code = $"100{code}", Result = "Order creation failed!" });
            }

            var payment = new PaymentCUDTO
            {
                OrderId = orderId,
                Amount = total,
                PaymentMethod = dto.PaymentMethod,
                PaymentDate = DateTime.Now,
                PaymentStatus = "Pending"
            };

            var result = await _paymentService.CreatePaymentAsync(payment);

            //await _cartService.ClearCartAsync(dto.AccountId);

            await _cartService.RemoveSelectedItemsAsync(selectedProductIds, dto.AccountId);

            return Ok(new APIResponse<object>
            {
                Code = "1000",
                Result = new
                {
                    Order = orderId,
                    Payments = result.Item1
                }
            });
        }
    }
}
