using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CS_ClothesStore.Controllers.Admin
{
    [Route("/Admin/[controller]/[action]")]
    public class ColorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public ColorController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
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

            var response = await _httpClient.GetAsync($"{_apiUrl}/Color/GetAll");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Can't get all Colors";
                return View(new List<ColorDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<List<ColorDTO>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var allColors = apiResponse?.Result;

            int totalItems = allColors.Count;
            var pageData = allColors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.StatusFilter = status;

            return View("~/Views/Admin/Color/Index.cshtml", pageData);
        }

        [HttpGet("/Admin/Color/Create")]
        public IActionResult Create()
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "You must be logged in to access this page.";
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Color/Create.cshtml", new ColorCUDTO());
            }

            return View("~/Views/Admin/Color/Create.cshtml", new ColorCUDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ColorCUDTO dto)
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
                var response = await _httpClient.PostAsync($"{_apiUrl}/Color/CreateColor", jsonContent);

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<APIResponse<ColorCUDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Code == "1000")
                {
                    ViewBag.Message = "Created successfully!";
                    ViewBag.MessageType = "success";
                    return RedirectToAction("Index", "Color");
                }

                ViewBag.Message = "Create failed";
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Color/Create.cshtml", dto);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Color/Create.cshtml", dto);

            }


        }

        [HttpGet("/Admin/Color/Edit/{id}")]
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

                var response = await _httpClient.GetAsync($"{_apiUrl}/Color/GetColorById/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get information Color.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Color/Edit.cshtml", new ColorCUDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<ColorCUDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null)
                {
                    ViewBag.Message = "The Color not exists.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Color/Edit.cshtml", new ColorCUDTO());
                }

                return View("~/Views/Admin/Color/Edit.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Color/Edit.cshtml", new ColorCUDTO());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ColorCUDTO colorCUDTO)
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

                var jsonContent = new StringContent(JsonSerializer.Serialize(colorCUDTO), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiUrl}/Color/UpdateColor", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Error: " + errorContent);

                    ViewBag.Message = "Update failed: " + errorContent;
                    ViewBag.MessageType = "danger";
                    return View("~/Views/Admin/Color/Edit.cshtml", colorCUDTO);
                }

                ViewBag.Message = "Color updated successfully!";
                ViewBag.MessageType = "success";
                return RedirectToAction("Index", "Color");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "danger";
                return View("~/Views/Admin/Color/Edit.cshtml", colorCUDTO);
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
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/Color/DeleteColor/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<APIResponse<int>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return RedirectToAction("Index", "Color");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
