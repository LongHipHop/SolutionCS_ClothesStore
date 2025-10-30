using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace CS_ClothesStore.Controllers.Admin
{
    [Route("Admin/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiUrl = "http://localhost:5013/api";

        public DashboardController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            //ViewBag.Message = "";
            //ViewBag.MessageType = "";

            //var token = HttpContext.Session.GetString("JWTToken");
            //var userJson = HttpContext.Session.GetString("UserInfo");

            //if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userJson))
            //{
            //    return RedirectToAction("Login", "Authentication");
            //}

            try
            {
                //Call CountAllAccounts
                var countAllAccountsResponse = await _httpClient.GetAsync($"{_apiUrl}/Account/CountAllAccounts");
                countAllAccountsResponse.EnsureSuccessStatusCode();
                var countAllAccountsJson = await countAllAccountsResponse.Content.ReadAsStringAsync();
                var countAllAccountResult = JsonSerializer.Deserialize<APIResponse<int>>(countAllAccountsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (countAllAccountResult == null || countAllAccountResult.Code != "1000")
                {
                    ViewBag.Message = "Can't get all accounts!";
                    ViewBag.MessageType = "error";
                    ViewBag.TotalAccounts = 0;
                }
                else
                {
                    ViewBag.TotalAccounts = countAllAccountResult.Result;
                }

                //Call CountUsersByWeek
                var countUsersByWeekResponse = await _httpClient.GetAsync($"{_apiUrl}/Account/CountAccountsByWeek");
                countUsersByWeekResponse.EnsureSuccessStatusCode();
                var countUsersByWeekJson = await countUsersByWeekResponse.Content.ReadAsStringAsync();
                var countUsersByWeekResult = JsonSerializer.Deserialize<APIResponse<AccountsByWeekDTO>>(countUsersByWeekJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (countUsersByWeekResult == null || countUsersByWeekResult.Code != "1000")
                {
                    ViewBag.Error = (string.IsNullOrEmpty(ViewBag.Error) ? "" : ViewBag.Error + " ") + "Unable to get user data by week.";
                    ViewBag.OldAccounts = 0;
                    ViewBag.NewAccounts = 0;
                }
                else
                {
                    ViewBag.OldAccounts = countUsersByWeekResult.Result.OldAccounts;
                    ViewBag.NewAccounts = countUsersByWeekResult.Result.NewAccounts;
                }

                return View("~/Views/Admin/Dashboard/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.TotalAccounts = 0;
                ViewBag.OldAccounts = 0;
                ViewBag.NewAccounts = 0;

                return View("~/Views/Admin/Dashboard/Index.cshtml");
            }
        }
    }
}
