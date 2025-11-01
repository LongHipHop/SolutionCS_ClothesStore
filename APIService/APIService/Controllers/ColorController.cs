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
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await _colorService.GetAll();
            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<ColorDTO>>
            {
                Code = code,
                Result = (IEnumerable<ColorDTO>)item.Item1
            };

            return Ok(response);
        }

        [HttpGet("GetColor/{id}")]
        public async Task<IActionResult> GetColor(int id)
        {
            var codeResult = await _colorService.GetColorById(id);

            string code = $"100{codeResult.Item2}";

            var response = new APIResponse<ColorDTO>
            {
                Code = code,
                Result = codeResult.Item1
            };
            return Ok(response);
        }

        [HttpPut("UpdateColor")]
        public async Task<IActionResult> UpdateColor([FromBody] ColorCUDTO color)
        {
            if (color == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                });
            }

            var codeResult = await _colorService.UpdateColor(color);

            string code = $"100{codeResult}";

            var response = new APIResponse<ColorCUDTO>
            {
                Code = code,
                Result = color
            };

            return Ok(response);
        }

        [HttpPost("CreateColor")]
        public async Task<IActionResult> CreateColor([FromBody] ColorCUDTO color)
        {
            if (color == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                });
            }
            var result = await _colorService.CreateColor(color);
            string code = $"100{result}";

            var response = new APIResponse<ColorCUDTO>
            {
                Code = code,
                Result = color
            };
            return Ok(response);
        }

        [HttpDelete("DeleteColor/{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var codeResult = await _colorService.DeleteColor(id);

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
