using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CS_ClothesStore.Controllers.Admin
{
    [Route("/Admin/[controller]/[action]")]
    public class ProductVariantController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public ProductVariantController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string status = "")
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var response = await _httpClient.GetAsync($"{_apiUrl}/ProductVariant/GetAll");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Can't get all product variants";
                return View(new List<ProductVariantDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<List<ProductVariantDTO>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var allProducts = apiResponse?.Result;

            int totalItems = allProducts.Count;
            var pageData = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.StatusFilter = status;

            return View("~/Views/Admin/ProductVariant/Index.cshtml", pageData);
        }

        [HttpGet("/Admin/ProductVariant/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            {
                ViewBag.Message = "Not logged in.";
                ViewBag.MessageType = "error";
                return RedirectToAction("Login", "Authentication");
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{_apiUrl}/ProductVariant/GetProductById/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get product variant information.";
                    ViewBag.MessageType = "error";

                    return View("~/Views/Admin/ProductVariant/Details.cshtml", new ProductVariantDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<ProductVariantDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null )
                {
                    ViewBag.Message = "No product found or wrong product variant type.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/ProductVariant/Details.cshtml", new ProductVariantDTO());
                }

                return View("~/Views/Admin/ProductVariant/Details.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/ProductVariant/Details.cshtml", new ProductVariantDTO());
            }
        }

        [HttpGet("/Admin/ProductVariant/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            {
                ViewBag.Message = "Not logged in.";
                ViewBag.MessageType = "error";
                return RedirectToAction("Login", "Authentication");
            }
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"{_apiUrl}/ProductVariant/GetProductVariantById/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get information Product variant.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/ProductVariant/Edit.cshtml", new ProductVariantCUDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<ProductVariantCUDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null)
                {
                    ViewBag.Message = "The Product variant not exists.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/ProductVariant/Edit.cshtml", new ProductVariantCUDTO());
                }

                return View("~/Views/Admin/ProductVariant/Edit.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/ProductVariant/Edit.cshtml", new ProductVariantCUDTO());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductVariantCUDTO ProductVariantCUDTO)
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Authentication");
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/ProductVariant/UpdateProductVariant", ProductVariantCUDTO);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Error: " + errorContent);

                    ViewBag.Message = "Update failed: " + errorContent;
                    ViewBag.MessageType = "danger";
                    return View("~/Views/Admin/ProductVariant/Edit.cshtml", ProductVariantCUDTO);
                }

                ViewBag.Message = "Product updated successfully!";
                ViewBag.MessageType = "success";
                return RedirectToAction("Index", "ProductVariant");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "danger";
                return View("~/Views/Admin/ProductVariant/Edit.cshtml", ProductVariantCUDTO);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Unauthorized" });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/ProductVariant/DeleteProductVariant/{id}");
                var json = await response.Content.ReadAsStringAsync();

                // Thay APIResponse<int> bằng object
                var result = JsonSerializer.Deserialize<APIResponse<object>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Code == "1000")
                    return Json(new { success = true, message = "Deleted successfully!" });
                else if (result?.Code == "1002")
                    return Json(new { success = false, message = "Product not found." });
                else
                    return Json(new { success = false, message = "Delete failed." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpGet("/Admin/ProductVariant/Create")]
        public IActionResult Create()
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "You must be logged in to access this page.";
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/ProductVariant/Create.cshtml", new ProductVariantCUDTO());
            }

            return View("~/Views/Admin/ProductVariant/Create.cshtml", new ProductVariantCUDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductVariantCUDTO dto)
        {
            var token = HttpContext.Session.GetString("JWTToken");


            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/ProductVariant/CreateProductVariant", dto);

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<APIResponse<ProductVariantCUDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result?.Code == "1000")
                return RedirectToAction("Index", "ProductVariant");

            ViewBag.Message = "❌ Create failed";
            ViewBag.MessageType = "error";
            return View("~/Views/Admin/ProductVariant/Create.cshtml", dto);
        }
    }
}
