﻿using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using CS_ClothesStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace CS_ClothesStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public ShoppingCartController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ShoppingCartViewModel();

            var userJson = HttpContext.Session.GetString("UserInfo");

            var user = JsonSerializer.Deserialize<AccountDTO>(userJson);

            var cart = await GetCartByAccountIdAsync(user.Id);
            viewModel.Cart = cart;
            return View(viewModel);
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

        [HttpPost]
        public async Task<IActionResult> ProceedToCheckout([FromBody] CheckoutDTO dto)
        {
            if (dto == null || dto.SelectedItems == null || !dto.SelectedItems.Any())
            {
                TempData["Error"] = "No items selected.";
                return RedirectToAction("Index");
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/Checkout/ProceedToCheckout", dto);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<APIResponse<object>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse?.Code == "1000")
                    {
                        TempData["Success"] = "Checkout successfully!";
                        var orderId = ((JsonElement)apiResponse.Result).GetProperty("Order").GetInt32();
                        return RedirectToAction("OrderSuccess", new { orderId });
                    }

                    TempData["Error"] = "Checkout failed: " + apiResponse?.Result;
                }
                else
                {
                    TempData["Error"] = "Can't connect with API!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Checkout error: {ex.Message}");
                TempData["Error"] = "Error occurred.";
            }

            return RedirectToAction("Index");
        }

    }
}
