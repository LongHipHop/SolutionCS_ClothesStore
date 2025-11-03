using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CS_ClothesStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public OrderController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> OrderSuccess(int orderId)
        {
            if (orderId <= 0)
            {
                TempData["Error"] = "Invalid order ID.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/Order/GetById/{orderId}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Cannot load order details.";
                    return RedirectToAction("Index", "ShoppingCart");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<OrderDTO>>(
                    responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (apiResponse?.Code == "1000" && apiResponse.Result != null)
                {
                    return View(apiResponse.Result);
                }

                TempData["Error"] = "Order not found.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading order success page: {ex.Message}");
                TempData["Error"] = "Something went wrong.";
            }

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
