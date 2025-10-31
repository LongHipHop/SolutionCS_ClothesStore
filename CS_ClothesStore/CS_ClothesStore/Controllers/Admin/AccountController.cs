using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CS_ClothesStore.Controllers.Admin
{
    [Route("Admin/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public AccountController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
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

            var response = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAll");

            if(!response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Can't get all accounts";
                return View(new List<AccountDTO>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<APIResponse<List<AccountDTO>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

            var allAccounts = apiResponse?.Result
                .Where(a => a.RoleName == "CUSTOMER")
                .ToList() ?? new List<AccountDTO>();

            if (!string.IsNullOrEmpty(status))
            {
                allAccounts = allAccounts
                    .Where(c => c.Status?.Equals(status, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }

            int totalItems = allAccounts.Count;
            var pageData = allAccounts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.StatusFilter = status;

            return View("~/Views/Admin/Account/Index.cshtml", pageData);
        }

        [HttpGet("/Admin/Account/Details/{id}")]
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

                var response = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountById/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get customer information.";
                    ViewBag.MessageType = "error";

                    return View("~/Views/Admin/Account/Details.cshtml", new AccountDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<AccountDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null || apiResponse.Result.RoleName != "CUSTOMER")
                {
                    ViewBag.Message = "No customer found or wrong customer type.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Account/Details.cshtml", new AccountDTO());
                }

                return View("~/Views/Admin/Account/Details.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Account/Details.cshtml", new AccountDTO());
            }
        }

        [HttpGet("/Admin/Account/Edit/{id}")]
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

                var response = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountById/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Can't get information account.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Account/Edit.cshtml", new AccountCUDTO());
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<AccountCUDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Result == null)
                {
                    ViewBag.Message = "The account not exists.";
                    ViewBag.MessageType = "error";
                    return View("~/Views/Admin/Account/Edit.cshtml", new AccountCUDTO());
                }

                return View("~/Views/Admin/Account/Edit.cshtml", apiResponse.Result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "error";
                return View("~/Views/Admin/Account/Edit.cshtml", new AccountCUDTO());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountCUDTO accountCUDTO, IFormFile? Img)
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

                // 🔹 Xử lý upload ảnh (nếu có)
                if (Img != null && Img.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Tạo tên file duy nhất
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Img.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Img.CopyToAsync(stream);
                    }

                    // Gán tên file vào model
                    accountCUDTO.Image = fileName;
                }
                else
                {
                    // Nếu không upload ảnh mới → giữ ảnh cũ
                    var currentUser = JsonSerializer.Deserialize<AccountCUDTO>(userJson);
                    accountCUDTO.Image = currentUser?.Image;
                }

                if (accountCUDTO.Password == null)
                {
                    var currentUser = JsonSerializer.Deserialize<AccountCUDTO>(userJson);
                    accountCUDTO.Password = currentUser.Password;
                    accountCUDTO.Status = currentUser.Status;
                    accountCUDTO.RoleName = currentUser.RoleName;
                }

                // 🔹 Gọi API để cập nhật
                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/Account/EditAccount", accountCUDTO);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Error: " + errorContent);

                    ViewBag.Message = "Update failed: " + errorContent;
                    ViewBag.MessageType = "danger";
                    return View("~/Views/Admin/Account/Edit.cshtml", accountCUDTO);
                }

                // 🔹 Thành công
                ViewBag.Message = "Account updated successfully!";
                ViewBag.MessageType = "success";
                return RedirectToAction("Index" , "Account");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "System error: " + ex.Message;
                ViewBag.MessageType = "danger";
                return View("~/Views/Admin/Account/Edit.cshtml", accountCUDTO);
            }
        }


    }
}
