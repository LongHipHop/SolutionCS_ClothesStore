using APIService.HttpResponse;
using APIService.Models.DTOs;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if(loginModel == null)
            {
                var errorResponse = new APIResponse<object>
                {
                    Code = "1004",
                    Result = null
                };
                return Ok(errorResponse);
            }

            var codeResult = await _authenticationService.Login(loginModel);

            string code = $"100{codeResult.Item2}";
            if(codeResult.Item2 == 2)
            {
                Unauthorized("Account or password incorrectly!");
                var response = new APIResponse<object>
                {
                    Code = code,
                    Result = null
                };
                return Ok(response);
            }
            else if(codeResult.Item2 == 3)
            {
                var response = new APIResponse<object>
                {
                    Code = code,
                    Result = null
                };
                return Ok(response);
            }
            else
            {
                var response = new APIResponse<ApplicationUser>
                {
                    Code = code,
                    Result = codeResult.Item1
                };
                return Ok(response);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
            var item = await _authenticationService.Logout(accessToken);
            string code = $"100{item}";

            var response = new APIResponse<object>
            {
                Code = code,
                Result = null
            };
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            var result = await _authenticationService.SendForGotPasswordEmailAsync(model.Email);
            if (!result)
                return BadRequest(new { message = "Email does not exist in the system." });

            return Ok( new {message = "Password recovery email has been sent."});
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var result = await _authenticationService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);

            if (!result)
                return BadRequest(new { message = "Token is invalid or expired." });

            return Ok(new { message = "Password reset successful!" });
        }
    }
}
