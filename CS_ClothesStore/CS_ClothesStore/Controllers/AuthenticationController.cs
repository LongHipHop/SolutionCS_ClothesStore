using CS_ClothesStore.HttpResponse;
using CS_ClothesStore.Models.DTOs;
using CS_ClothesStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CS_ClothesStore.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly string _apiUrl = "http://localhost:5013/api";
        public AuthenticationController(HttpClient httpClient, IWebHostEnvironment environment)
        {
            _httpClient = httpClient;
            _environment = environment;
        }

        public IActionResult Login()
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";

            var token = HttpContext.Session.GetString("JWTToken");

            if (token != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model, [FromForm] bool rememberMe)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiUrl}/Authentication/Login", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Query error";
                    ViewBag.MessageType = "error";
                    return View(model);
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<APIResponse<ApplicationUser>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                string codeResult = apiResponse.Code;
                var item = apiResponse.Result;

                if(codeResult == "1002")
                {
                    ViewBag.Message = "Invalid email or password.";
                    ViewBag.MessageType = "error";
                    return View(model);
                }
                else if (codeResult == "1001")
                {
                    ViewBag.Message = "System error occurred";
                    ViewBag.MessageType = "error";
                    return View(model);
                }
                else if(codeResult == "1003")
                {
                    ViewBag.Message = "Your account has been locked.";
                    ViewBag.MessageType = "error";
                    return View(model);
                }
                else if(codeResult == "1000")
                {
                    ViewBag.Message = "Login Successful";
                    ViewBag.MessageType = "success";
                    HttpContext.Session.SetString("JWTToken", item.Token);
                    HttpContext.Session.SetString("RefreshToken", item.RefreshToken);
                    HttpContext.Session.SetString("TokenExpiry", item.RefreshTokenExpiryTime.ToString());

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", item.Token);

                    if (item.Roles != null && item.Roles.Any())
                    {
                        var mainRole = item.Roles.First();
                        HttpContext.Session.SetString("RoleID", mainRole.ToString());
                        HttpContext.Session.SetString("RoleName", mainRole.RoleName);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Email),
                            new Claim(ClaimTypes.Role, mainRole.RoleName)

                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    }
                    else
                    {
                        Console.WriteLine("No roles found for user");
                    }

                    var responseGetUser = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountByEmail/{model.Email}");
                    json = await responseGetUser.Content.ReadAsStringAsync();
                    var apiResponseGetUser = JsonSerializer.Deserialize<APIResponse<AccountDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var itemUser = apiResponseGetUser.Result;

                    HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(itemUser));
                    HttpContext.Session.SetString("AccountID", itemUser.Id.ToString());

                    if (rememberMe)
                    {
                        CookieOptions options = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(7),
                            HttpOnly = true,
                            Secure = false,
                            IsEssential = true
                        };
                        Response.Cookies.Append("RememberEmail", model.Email, options);
                        Response.Cookies.Append("RememberPass", model.Password, options);
                    }
                    else
                    {
                        Response.Cookies.Delete("RememberEmail");
                        Response.Cookies.Delete("RememberPass");
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Query error";
                    ViewBag.MessageType = "error";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.MessageType = "error";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewBag.Message = "";
            ViewBag.MessageType = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordModel model)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiUrl}/Authentication/forgot-password", jsonContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Password recovery email has been sent.";
                ViewBag.MessageType = "success";
            }
            else
            {
                ViewBag.Message = "Email dose not exist.";
                ViewBag.MessageType = "error";
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordModel { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordModel model)
        {
            if(model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Message = "Password confirmation does not match.";
                ViewBag.MessageType = "error";
                return View(model);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiUrl}/Authentication/reset-password", jsonContent);
            var result = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Password reset successful!";
                ViewBag.MessageType = "success";
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                ViewBag.Message = "Token is invalid or expired.";
                ViewBag.MessageType = "danger";
            }
            return View(model);
        }
    }
}
