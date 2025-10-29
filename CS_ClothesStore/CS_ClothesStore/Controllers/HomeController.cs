using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models;
using CS_ClothesStore.Models.DTOs;
using CS_ClothesStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Text.Json;

namespace CS_ClothesStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public HomeController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            ////if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            ////{
            ////    return RedirectToAction("Login", "Authentication");
            ////}
            ///
            var viewModel = new HomeViewModel
            {
                Products = new List<ProductDTO>(),
                Categories = new List<CategoryDTO>()
            };

            var tasks = new List<Task>();

            Task getProductsTask = Task.CompletedTask;
            Task getCategoriesTask = Task.CompletedTask;
    

            getProductsTask = GetProductAsync();
            getCategoriesTask = GetCategoriesAsync();
 
            tasks.Add(getProductsTask);
            tasks.Add(getCategoriesTask);

            await Task.WhenAll(tasks);

            viewModel.Products = ((Task<List<ProductDTO>>)getProductsTask).Result ?? new List<ProductDTO>();
            viewModel.Categories = ((Task<List<CategoryDTO>>)getCategoriesTask).Result ?? new List<CategoryDTO>();


            //Get Details Product
            var detailsTask = viewModel.Products.Select(p => GetProductByIdAsync(p.Id));
            var productDetails = await Task.WhenAll(detailsTask);

            viewModel.ProductDetails = productDetails.Where(d => d != null).ToList();

            

            return View(viewModel);
        }

        private async Task<List<ProductDTO>> GetProductAsync()
        {
            try
            {
                var productResponse = await _httpClient.GetAsync($"{_apiUrl}/Product/GetAll");

                if (productResponse.IsSuccessStatusCode)
                {
                    var productJson = await productResponse.Content.ReadAsStringAsync();
                    var productApiResponse = JsonSerializer.Deserialize<APIResponse<List<ProductDTO>>>(productJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if(productApiResponse?.Code == "1000" && productApiResponse.Result != null)
                    {
                        return productApiResponse.Result.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<ProductDTO>();
        }

        private async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            try
            {
                var categoryResponse = await _httpClient.GetAsync($"{_apiUrl}/Category/GetAll");
                if (categoryResponse.IsSuccessStatusCode)
                {
                    var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                    var categoryApiResponse = JsonSerializer.Deserialize<APIResponse<List<CategoryDTO>>>(categoryJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if(categoryApiResponse?.Code == "1000")
                    {
                        return categoryApiResponse.Result.OrderByDescending(c => c.CategoryName).Take(3).ToList();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<CategoryDTO>();
        }

        private async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/Product/GetProductById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var productJson = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<APIResponse<ProductDTO>>(productJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse?.Code == "1000" && apiResponse.Result != null)
                    {
                        return apiResponse.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private async Task<CartDTO?> GetCartByAccountIdAsync(int accountId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/Cart/GetCart/{accountId}");

                if (response.IsSuccessStatusCode)
                {
                    var cartJson = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<APIResponse<CartDTO>>(cartJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse?.Code == "1000" && apiResponse?.Result != null)
                    {
                        return apiResponse.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when getting cart: {ex.Message}");
            }

            return null;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
