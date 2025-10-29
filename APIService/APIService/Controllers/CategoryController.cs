using APIService.HttpResponse;
using APIService.Models.DTOs;
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
    }
}
