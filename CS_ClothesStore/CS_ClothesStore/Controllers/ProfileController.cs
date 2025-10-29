using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS_ClothesStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public ProfileController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var user = JsonSerializer.Deserialize<AccountDTO>(userJson);

            if(user == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var response = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountByEmail/{user.Email}");
            if(!response.IsSuccessStatusCode)
            {
                return View(null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<AccountDTO>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var updatedUser = apiResponse?.Result;
            if (updatedUser == null)
                return View(null);

            return View(updatedUser);
        }
    }
}
