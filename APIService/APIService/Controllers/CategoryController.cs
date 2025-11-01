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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await _categoryService.GetAll();

            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<CategoryDTO>>
            {
                Code = code,
                Result = (IEnumerable<CategoryDTO>)item.Item1
            };
            return Ok(response);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var codeResult = await _categoryService.GetCategoryById(id);

            string code = $"100{codeResult.Item2}";

            var response = new APIResponse<CategoryDTO>
            {
                Code = code,
                Result = codeResult.Item1
            };
            return Ok(response);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryCUDTO Category)
        {
            if (Category == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                });
            }

            var codeResult = await _categoryService.UpdateCategory(Category);

            string code = $"100{codeResult}";

            var response = new APIResponse<CategoryCUDTO>
            {
                Code = code,
                Result = Category
            };

            return Ok(response);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCUDTO Category)
        {
            if (Category == null)
            {
                return Ok(new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                });
            }
            var result = await _categoryService.CreateCategory(Category);
            string code = $"100{result}";

            var response = new APIResponse<CategoryCUDTO>
            {
                Code = code,
                Result = Category
            };
            return Ok(response);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var codeResult = await _categoryService.DeleteCategory(id);

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
