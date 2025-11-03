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
    public class ShippingProviderController : ControllerBase
    {
        private readonly IShippingProviderService _shippingProviderService;

        public ShippingProviderController(IShippingProviderService shippingProviderService)
        {
            _shippingProviderService = shippingProviderService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await _shippingProviderService.GetAll();

            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<ShippingProviderDTO>>
            {
                Code = code,
                Result = (IEnumerable<ShippingProviderDTO>)item.Item1
            };

            return Ok(response);
        }
    }
}
