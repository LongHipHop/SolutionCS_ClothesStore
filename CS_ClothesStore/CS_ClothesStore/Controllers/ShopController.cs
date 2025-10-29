using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CS_ClothesStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public ShopController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var productResponse = await _httpClient.GetAsync($"{_apiUrl}/Product/GetAll");
            var list = new List<ProductDTO>();
            if (!productResponse.IsSuccessStatusCode)
            {
                TempData["Message"] = "Can't get Product List!";
                return View(new List<ProductDTO>());
            }

            var productJson = await productResponse.Content.ReadAsStringAsync();
            var productApiResponse = JsonSerializer.Deserialize<APIResponse<IEnumerable<ProductDTO>>>(productJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var productList = productApiResponse?.Result?.ToList() ?? new List<ProductDTO>();


            return View(productList);
        }
    }
}
