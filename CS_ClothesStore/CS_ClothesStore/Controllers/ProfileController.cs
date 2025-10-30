using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
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

            if (user == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var response = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountByEmail/{user.Email}");
            if (!response.IsSuccessStatusCode)
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

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            if (!ModelState.IsValid)
            {
                return View(null);
            }

            var userJson = HttpContext.Session.GetString("UserInfo");
            if (string.IsNullOrEmpty(userJson) || string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var user = JsonSerializer.Deserialize<AccountCUDTO>(userJson);
            if (user == null) return RedirectToAction("Login", "Authentication");

            var response = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountByEmail/{user.Email}");
            if (!response.IsSuccessStatusCode)
            {
                return View(null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<AccountCUDTO>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(apiResponse?.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountCUDTO model)
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Authentication");
            }

            //if(!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            if(model.ImgFile == null || model.ImgFile.Length == 0)
            {
                var user = string.IsNullOrEmpty(userJson) 
                    ? null : JsonSerializer.Deserialize<AccountCUDTO>(userJson);

                if(user != null)
                {
                    model.Image = user.Image;
                }
            }
            else
            {
                var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/avatars");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImgFile.FileName);
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImgFile.CopyToAsync(stream);
                }

                model.Image = uniqueFileName;
            }

            if (model.Password == null)
            {
                var currentUser = JsonSerializer.Deserialize<AccountCUDTO>(userJson);
                model.Password = currentUser.Password;
                model.Status = currentUser.Status;
                model.RoleName = currentUser.RoleName;
            }

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/Account/EditAccount", model);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API error: " + errorContent);
                ModelState.AddModelError("", $"Update failed: {errorContent}");
                return View(model);
            }

            var refetchResponse = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountByEmail/{model.Email}");
            if(refetchResponse.IsSuccessStatusCode)
            {
                var content = await refetchResponse.Content.ReadAsStringAsync();
                var apiRefetch = JsonSerializer.Deserialize<APIResponse<AccountCUDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(apiRefetch?.Result != null)
                {
                    HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(apiRefetch.Result));
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userJson = HttpContext.Session.GetString("UserInfo");
            if(string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var user = JsonSerializer.Deserialize<AccountCUDTO>(userJson);
            var model = new ChangePasswordDTO { Id = user.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");
            var userJson = HttpContext.Session.GetString("UserInfo");

            if(string.IsNullOrEmpty(userJson) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Authentication");
            }

            var user = JsonSerializer.Deserialize<AccountDTO>(userJson);
            model.Id = user.Id;

            if (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                ViewBag.Message = "Please fill all required fields.";
                ViewBag.MessageType = "danger";
                return View(model);
            }

            var request = new HttpRequestMessage(HttpMethod.Put, $"{_apiUrl}/Account/EditPassword");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if(!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.Message = $"Failed: {errorContent}";
                ViewBag.MessageType = "danger";
                return View(model);
            }

            ViewBag.Message = "Password changed successfully!";
            ViewBag.MessageType = "success";

            return View(model);
        }
    }
}
