using APIService.HttpResponse;
using APIService.Models.DTOs;
using APIService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var item = await _accountService.GetAll();
            string code = $"100{item.Item2}";

            var response = new APIResponse<IEnumerable<AccountDTO>>
            {
                Code = code,
                Result = (IEnumerable<AccountDTO>)item.Item1
            };
            return Ok(response);
        }

        [HttpGet("GetAccountById/{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var item = await _accountService.GetAccountById(id);
            string code = $"100{item.Item2}";

            var response = new APIResponse<AccountDTO>
            {
                Code = code,
                Result = item.Item1
            };
            return Ok(response);
        }

        [HttpGet("GetAccountByEmail/{email}")]
        public async Task<IActionResult> GetAccountByEmail(string email)
        {
            var item = await _accountService.GetAccountByEmail(email);
            string code = $"100{item.Item2}";

            var response = new APIResponse<AccountDTO>
            {
                Code = code,
                Result = item.Item1
            };
            return Ok(response);
        }

        [HttpPut("EditAccount")]
        public async Task<IActionResult> EditAccount(AccountCUDTO accountUpdate)
        {
            if(accountUpdate == null)
            {
                var errorResponse = new APIResponse<object>
                {
                    Code = "1003",
                    Result = null
                };
                return Ok(errorResponse);
            }

            var codeResult = await _accountService.UpdateAsync(accountUpdate);

            string code = $"100{codeResult}";

            var response = new APIResponse<AccountCUDTO>
            {
                Code = code,
                Result = accountUpdate
            };

            return Ok(response);
        }

        [HttpPut("EditPassword")]
        public async Task<IActionResult> EditPassword([FromBody] ChangePasswordDTO model)
        {
            if(model == null || model.Id <= 0 || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest(new APIResponse<object>
                {
                    Code = "1003",
                    Result = "Invalid data"
                });
            }

            var codeResult = await _accountService.UpdatePassword(model.OldPassword, model.NewPassword, model.Id);

            string code = $"100{codeResult}";

            return codeResult switch
            {
                0 => Ok(new APIResponse<object> { Code = code, Result = "Password updated successfully" }),
                2 => NotFound(new APIResponse<object> { Code = code, Result = "Account not found" }),
                3 => BadRequest(new APIResponse<object> { Code = code, Result = "Old password incorrect" }),
                _ => StatusCode(500, new APIResponse<object> { Code = code, Result = "Server error" })
            };
        }

        [HttpDelete("DeleteAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if(id == null)
            {
                var errorResponse = new APIResponse<object>
                {
                    Code = "1003",
                    Result = null
                };
            }

            var codeResult = await _accountService.DeleteAsync(id);

            string code = $"100{codeResult}";

            var response = new APIResponse<object>
            {
                Code = code,
                Result = null
            };
            return Ok(response);
        }

        [HttpGet("CountAllAccounts")]
        public async Task<IActionResult> CountAllAccounts()
        {
            var count = await _accountService.CountAllUsersAsync();

            string code = $"1000";

            var response = new APIResponse<object>
            {
                Code = code,
                Result = count
            }; 
            return Ok(response);
        }

        [HttpGet("CountAccountsByWeek")]
        public async Task<IActionResult> CountAccountsByWeek()
        {
            var (oldAccounts, newAccounts) = await _accountService.CountUsersByWeekAsync();

            string code = $"1000";

            var response = new APIResponse<object>
            {
                Code = code,
                Result = new { oldAccounts, newAccounts }
            };

            return Ok(response);
        }
        
    }
}
