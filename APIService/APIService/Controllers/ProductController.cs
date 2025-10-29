using APIService.HttpResponse;
using APIService.Models.DTOs;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await _productService.GetAll();
            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<ProductDTO>>
            {
                Code = code,
                Result = (IEnumerable<ProductDTO>)item.Item1
            };
            return Ok(response);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var codeResult = await _productService.GetProductById(id);

            string code = $"100{codeResult.Item2}";

            var response = new APIResponse<ProductDTO>
            {
                Code = code,
                Result = codeResult.Item1
            };
            return Ok(response);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCUDTO product)
        {
            if(product == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1003",
                    Result = null
                });
            }
            var result = await _productService.CreateProduct(product);
            string code = $"100{result}";

            var response = new APIResponse<ProductCUDTO>
            {
                Code = code,
                Result = product
            };
            return Ok(response);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductCUDTO product)
        {
            if( product == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1003",
                    Result = null
                });
            }

            var codeResult = await _productService.UpdateProduct(product);

            string code = $"100{codeResult}";

            var response = new APIResponse<ProductCUDTO>
            {
                Code = code,
                Result = product
            };

            return Ok(response);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var codeResult = await _productService.DeleteProduct(id);

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
