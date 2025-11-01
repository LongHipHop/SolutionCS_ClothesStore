using APIService.HttpResponse;
using APIService.Models.DTOs;
using APIService.Service.Implementations;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService productVariantService;

        public ProductVariantController(IProductVariantService productVariantService)
        {
            this.productVariantService = productVariantService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await productVariantService.GetAll();
            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<ProductVariantDTO>>
            {
                Code = code,
                Result = (IEnumerable<ProductVariantDTO>)item.Item1
            };

            return Ok(response);
        }

        [HttpGet("GetProductVariant/{id}")]
        public async Task<IActionResult> GetProductVariant(int id)
        {
            var codeResult = await productVariantService.GetProductVariantById(id);

            string code = $"100{codeResult.Item2}";

            var response = new APIResponse<ProductVariantDTO>
            {
                Code = code,
                Result = codeResult.Item1
            };
            return Ok(response);
        }

        [HttpPut("UpdateProductVariant")]
        public async Task<IActionResult> UpdateProductVariant([FromBody] ProductVariantCUDTO product)
        {
            if (product == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                });
            }

            var codeResult = await productVariantService.UpdateProductVariant(product);

            string code = $"100{codeResult}";

            var response = new APIResponse<ProductVariantCUDTO>
            {
                Code = code,
                Result = product
            };

            return Ok(response);
        }

        [HttpPost("CreateProductVariant")]
        public async Task<IActionResult> CreateProductVariant([FromBody] ProductVariantCUDTO product)
        {
            if (product == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1003",
                    Result = null
                });
            }
            var result = await productVariantService.CreateProductVariant(product);
            string code = $"100{result}";

            var response = new APIResponse<ProductVariantCUDTO>
            {
                Code = code,
                Result = product
            };
            return Ok(response);
        }

        [HttpDelete("DeleteProductVariant/{id}")]
        public async Task<IActionResult> DeleteProductVariant(int id)
        {
            var codeResult = await productVariantService.DeleteProductVariant(id);

            string code = $"100{codeResult}";

            var response = new APIResponse<object>
            {
                Code = code,
                Result = null
            };

            return Ok(response);
        }
    }
}
