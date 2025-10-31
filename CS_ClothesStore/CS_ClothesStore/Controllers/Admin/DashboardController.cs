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

                var countAllProductResponse = await _httpClient.GetAsync($"{_apiUrl}/Product/CountAllProducts");
                countAllProductResponse.EnsureSuccessStatusCode();
                var countAllProductsJson = await countAllProductResponse.Content.ReadAsStringAsync();
                var countAllProductResult = JsonSerializer.Deserialize<APIResponse<int>>(countAllProductsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (countAllProductResult == null || countAllProductResult.Code != "1000")
                {
                    ViewBag.Message = "Can't get all products!";
                    ViewBag.MessageType = "error";
                    ViewBag.TotalProducts = 0;
                }
                else
                {
                    ViewBag.TotalProducts = countAllProductResult.Result;
                }

                var countAllUnprocessedPayment = await _httpClient.GetAsync($"{_apiUrl}/Payment/CountPaymentUnprocessed");
                countAllUnprocessedPayment.EnsureSuccessStatusCode();
                var countAllUnprocessedPaymentJson = await countAllUnprocessedPayment.Content.ReadAsStringAsync();
                var countAllUnprocessedPaymentResult = JsonSerializer.Deserialize<APIResponse<int>>(countAllUnprocessedPaymentJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (countAllUnprocessedPaymentResult == null || countAllUnprocessedPaymentResult.Code != "1000")
                {
                    ViewBag.Message = "Can't get all unprocessed payment!";
                    ViewBag.MessageType = "error";
                    ViewBag.TotalUnprocessedPayment = 0;
                }
                else
                {
                    ViewBag.TotalUnprocessedPayment = countAllUnprocessedPaymentResult.Result;
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


                var revenueSummaryResponse = await _httpClient.GetAsync($"{_apiUrl}/Payment/earnings/summary");
                revenueSummaryResponse.EnsureSuccessStatusCode();
                var revenueSummaryJson = await revenueSummaryResponse.Content.ReadAsStringAsync();
                var revenueSummary = JsonSerializer.Deserialize<dynamic>(revenueSummaryJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.DailyRevenue = revenueSummary?.GetProperty("daily").GetDecimal() ?? 0;
                ViewBag.WeeklyRevenue = revenueSummary?.GetProperty("weekly").GetDecimal() ?? 0;
                ViewBag.MonthlyRevenue = revenueSummary?.GetProperty("monthly").GetDecimal() ?? 0;
                ViewBag.YearlyRevenue = revenueSummary?.GetProperty("yearly").GetDecimal() ?? 0;


                var dailyRevenueResponse = await _httpClient.GetAsync($"{_apiUrl}/Payment/revenue/daily");
                dailyRevenueResponse.EnsureSuccessStatusCode();
                var dailyRevenueJson = await dailyRevenueResponse.Content.ReadAsStringAsync();
                var dailyRevenue = JsonSerializer.Deserialize<dynamic>(dailyRevenueJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.TodayRevenue = dailyRevenue?.GetProperty("totalRevenue").GetDecimal() ?? 0;
                ViewBag.TodayCount = dailyRevenue?.GetProperty("paymentCount").GetInt32() ?? 0;

                return View("~/Views/Admin/Dashboard/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.TotalAccounts = 0;
                ViewBag.OldAccounts = 0;
                ViewBag.NewAccounts = 0;
                ViewBag.DailyRevenue = 0;
                ViewBag.WeeklyRevenue = 0;
                ViewBag.MonthlyRevenue = 0;
                ViewBag.YearlyRevenue = 0;
                ViewBag.TodayRevenue = 0;
                ViewBag.TodayCount = 0;

                return View("~/Views/Admin/Dashboard/Index.cshtml");
            }
        }

        public async Task<IActionResult> Earnings()
        {
            return View("~/Views/Admin/Dashboard/Earnings.cshtml");
        }
    }
}
