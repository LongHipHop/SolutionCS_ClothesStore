using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CS_ClothesStore.Controllers.Admin
{
    [Route("/Admin/[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public CategoryController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
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

            var response = await _httpClient.GetAsync($"{_apiUrl}/Category/GetAll");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Can't get all Categories";
                return View(new List<CategoryDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<List<CategoryDTO>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var allCategorys = apiResponse?.Result;

            int totalItems = allCategorys.Count;
            var pageData = allCategorys
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.StatusFilter = status;

            return View("~/Views/Admin/Category/Index.cshtml", pageData);
        }

        [HttpGet("/Admin/Category/Create")]
        public IActionResult Create()
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "You must be logged in to access this page.";
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Category/Create.cshtml", new CategoryCUDTO());
            }

            return View("~/Views/Admin/Category/Create.cshtml", new CategoryCUDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCUDTO dto)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "Not logged in";
                ViewBag.MessageType = "error";
                return RedirectToAction("Login", "Authentication");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiUrl}/Category/CreateCategory", jsonContent);

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<APIResponse<CategoryCUDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Code == "1000")
                {
                    ViewBag.Message = "Created successfully!";
                    ViewBag.MessageType = "success";
                    return RedirectToAction("Index", "Category");
                }

                ViewBag.Message = "Create failed";
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Category/Create.cshtml", dto);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Category/Create.cshtml", dto);

            }


        }

        [HttpGet("/Admin/Category/Edit/{id}")]
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

                var response = await _httpClient.GetAsync($"{_apiUrl}/Category/GetCategoryById/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get information Category.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Category/Edit.cshtml", new CategoryCUDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<CategoryCUDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null)
                {
                    ViewBag.Message = "The Category not exists.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Category/Edit.cshtml", new CategoryCUDTO());
                }

                return View("~/Views/Admin/Category/Edit.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Category/Edit.cshtml", new CategoryCUDTO());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryCUDTO CategoryCUDTO)
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

                var jsonContent = new StringContent(JsonSerializer.Serialize(CategoryCUDTO), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiUrl}/Category/UpdateCategory", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Error: " + errorContent);

                    ViewBag.Message = "Update failed: " + errorContent;
                    ViewBag.MessageType = "danger";
                    return View("~/Views/Admin/Category/Edit.cshtml", CategoryCUDTO);
                }

                ViewBag.Message = "Category updated successfully!";
                ViewBag.MessageType = "success";
                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "danger";
                return View("~/Views/Admin/Category/Edit.cshtml", CategoryCUDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/Category/DeleteCategory/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<APIResponse<int>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
