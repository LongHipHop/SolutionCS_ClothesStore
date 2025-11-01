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
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await _sizeService.GetAll();
            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<SizeDTO>>
            {
                Code = code,
                Result = (IEnumerable<SizeDTO>)item.Item1
            };

            return Ok(response);
        }

        [HttpGet("GetSizeById/{id}")]
        public async Task<IActionResult> GetSize(int id)
        {
            var codeResult = await _sizeService.GetSizeById(id);

            string code = $"100{codeResult.Item2}";

            var response = new APIResponse<SizeDTO>
            {
                Code = code,
                Result = codeResult.Item1
            };
            return Ok(response);
        }

        [HttpPut("UpdateSize")]
        public async Task<IActionResult> UpdateSize([FromBody] SizeCUDTO size)
        {
            if (size == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                });
            }

            var codeResult = await _sizeService.UpdateSize(size);

            string code = $"100{codeResult}";

            var response = new APIResponse<SizeCUDTO>
            {
                Code = code,
                Result = size
            };

            return Ok(response);
        }

        [HttpPost("CreateSize")]
        public async Task<IActionResult> CreateSize([FromBody] SizeCUDTO size)
        {
            if (size == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                });
            }
            var result = await _sizeService.CreateSize(size);
            string code = $"100{result}";

            var response = new APIResponse<SizeCUDTO>
            {
                Code = code,
                Result = size
            };
            return Ok(response);
        }

        [HttpDelete("DeleteSize/{id}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var codeResult = await _sizeService.DeleteSize(id);

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
