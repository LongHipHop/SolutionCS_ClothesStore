using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CS_ClothesStore.Controllers.Admin
{
    [Route("/Admin/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";
        
        public OrderController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
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

            var response = await _httpClient.GetAsync($"{_apiUrl}/Order/GetAll");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Can't get all Orders";
                return View(new List<OrderDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<List<OrderDTO>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var allOrders = apiResponse?.Result;

            int totalItems = allOrders.Count;
            var pageData = allOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.StatusFilter = status;

            return View("~/Views/Admin/Order/Index.cshtml", pageData);
        }

        [HttpGet("/Admin/Order/Details/{id}")]
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

                var response = await _httpClient.GetAsync($"{_apiUrl}/Order/GetOrderById/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get Order information.";
                    ViewBag.MessageType = "error";

                    return View("~/Views/Admin/Order/Details.cshtml", new OrderDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<OrderDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null)
                {
                    ViewBag.Message = "No Order found or wrong Order type.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Order/Details.cshtml", new OrderDTO());
                }

                return View("~/Views/Admin/Order/Details.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Order/Details.cshtml", new OrderDTO());
            }
        }
    }
}
