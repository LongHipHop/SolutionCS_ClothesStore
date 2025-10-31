using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CS_ClothesStore.Controllers.Admin
{
    [Route("/Admin/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public ProductController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
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

            var response = await _httpClient.GetAsync($"{_apiUrl}/Product/GetAll");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Can't get all products";
                return View(new List<ProductDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<List<ProductDTO>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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

            return View("~/Views/Admin/Product/Index.cshtml", pageData);
        }

        [HttpGet("/Admin/Product/Details/{id}")]
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

                var response = await _httpClient.GetAsync($"{_apiUrl}/Product/GetProductById/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get product information.";
                    ViewBag.MessageType = "error";

                    return View("~/Views/Admin/Product/Details.cshtml", new ProductDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<ProductDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null )
                {
                    ViewBag.Message = "No product found or wrong product type.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Product/Details.cshtml", new ProductDTO());
                }

                return View("~/Views/Admin/Product/Details.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Product/Details.cshtml", new ProductDTO());
            }
        }

        [HttpGet("/Admin/Product/Edit/{id}")]
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

                var response = await _httpClient.GetAsync($"{_apiUrl}/Product/GetProductById/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get information Product.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Product/Edit.cshtml", new ProductCUDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<ProductCUDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null)
                {
                    ViewBag.Message = "The Product not exists.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Product/Edit.cshtml", new ProductCUDTO());
                }

                return View("~/Views/Admin/Product/Edit.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Product/Edit.cshtml", new ProductCUDTO());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCUDTO ProductCUDTO, IFormFile? Img)
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

                if (Img != null && Img.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

   
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Img.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Img.CopyToAsync(stream);
                    }

                    ProductCUDTO.Image = fileName;
                }
                else
                {
                    var currentUser = JsonSerializer.Deserialize<ProductCUDTO>(userJson);
                    ProductCUDTO.Image = currentUser?.Image;
                }

                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/Product/UpdateProduct", ProductCUDTO);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Error: " + errorContent);

                    ViewBag.Message = "Update failed: " + errorContent;
                    ViewBag.MessageType = "danger";
                    return View("~/Views/Admin/Product/Edit.cshtml", ProductCUDTO);
                }

                ViewBag.Message = "Product updated successfully!";
                ViewBag.MessageType = "success";
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "danger";
                return View("~/Views/Admin/Product/Edit.cshtml", ProductCUDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Unauthorized"});
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/DeleteProduct/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<APIResponse<int>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message});
            }
        }


        [HttpGet("/Admin/Product/Create")]
        public IActionResult Create()
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "You must be logged in to access this page.";
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Product/Create.cshtml", new ProductCUDTO());
            }

            return View("~/Views/Admin/Product/Create.cshtml", new ProductCUDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCUDTO dto)
        {
            var token = HttpContext.Session.GetString("JWTToken");

            // 🟢 Nếu có ảnh thì lưu trước
            if (dto.ImgFile != null && dto.ImgFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImgFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImgFile.CopyToAsync(stream);
                }

                dto.Image = fileName;
            }

            // 🟡 Gửi JSON qua API (FromBody)
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/Product/CreateProduct", dto);

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<APIResponse<ProductCUDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result?.Code == "1000")
                return RedirectToAction("Index", "Product");

            ViewBag.Message = "❌ Create failed";
            ViewBag.MessageType = "error";
            return View("~/Views/Admin/Product/Create.cshtml", dto);
        }



    }
}
