using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APIService.Service.Implementations
{
    public class AuthenticationService : Interface.IAuthenticationService
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepostiory _accountRepostiory;
        private readonly IPasswordResetRepository _passwordResetRepository;
        private IConfiguration _configuration;
        private IMapper _mapper;

        public AuthenticationService(IAccountService accountService, IAccountRepostiory accountRepostiory, IPasswordResetRepository passwordResetRepository, IConfiguration configuration, IMapper mapper)
        {
            _accountService = accountService;
            _accountRepostiory = accountRepostiory;
            _passwordResetRepository = passwordResetRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public string CreateToken(AccountDTO user)
        {
            var screcretkey = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(screcretkey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]);
            var expiration = DateTime.Now.AddMinutes(expirationMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_version", user.TokenVersion.ToString())
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: credentials);

            var isSuccess = new JwtSecurityTokenHandler().WriteToken(token);

            return isSuccess;
        }

        public Task<string> GenerateRefreshToken()
        {
            var ramdomNumber = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(ramdomNumber);
            }

            return Task.FromResult(Convert.ToBase64String(ramdomNumber));
        }

        public async Task<(ApplicationUser, int)> Login(LoginModel loginModel)
        {
            var userExist = await _accountService.GetAccountByLogin(loginModel);
            if (userExist.Item2 == 0)
            {
                userExist.Item1.TokenVersion++;
                userExist.Item1.RefreshToken = await this.GenerateRefreshToken();
                userExist.Item1.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

                var accessToken = this.CreateToken(userExist.Item1);

                var updateAccount = _mapper.Map<AccountCUDTO>(userExist.Item1);
                updateAccount.TokenVersion = userExist.Item1.TokenVersion;
                updateAccount.RefreshTokenExpiryTime = userExist.Item1.RefreshTokenExpiryTime;
                updateAccount.RefreshToken = userExist.Item1.RefreshToken;

                await _accountService.UpdateTokenOnLoginAsync(updateAccount);

                int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]);
                var expiration = DateTime.Now.AddDays(expirationMinutes);

                return (
                    new ApplicationUser
                    {
                        TokenVersion = userExist.Item1.TokenVersion,
                        RefreshToken = userExist.Item1.RefreshToken,
                        Token = accessToken,
                        RefreshTokenExpiryTime = expiration,
                    }, 0);
            }
            else
            {
                return (new(), userExist.Item2);
            }
        }

        public async Task<int> Logout(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if(jwtToken != null)
                {
                    var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                    var username = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                    var item = await _accountService.GetAccountByEmail(username);
                    if(item.Item2 != 0)
                    {
                        return item.Item2;
                    }

                    if(!string.IsNullOrEmpty(jti) && username == item.Item1.Email)
                    {
                        var accountUpdate = _mapper.Map<AccountCUDTO>(item.Item1);
                        accountUpdate.TokenVersion = accountUpdate.TokenVersion + 1;
                        var isSuccess = await _accountService.UpdateTokenOnLoginAsync(accountUpdate);
                        if(isSuccess != 0)
                        {
                            return isSuccess;
                        }
                    }
                    return 0;
                }
                return 2;
            }catch (Exception ex)
            {
                return 1;
            }
        }

        public async Task<(ApplicationUser, int)> RefreshToken(RequestTokenModel requestTokenModel, string oldAccessToken)
        {
            var user = await _accountService.GetAccountByToken(requestTokenModel.RefreshToken);
            if(user.Item2 != 0)
            {
                return (new(), user.Item2);
            }

            if(user.Item1.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return (new(), 3);
            }

            if (!string.IsNullOrEmpty(oldAccessToken))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(oldAccessToken) as JwtSecurityToken;
                    if(jwtToken != null)
                    {
                        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                        if(!string.IsNullOrEmpty(jti) && username == user.Item1.Email)
                        {
                            int expirationMinuesBlack = int.Parse(_configuration["Jwt:ExpirationMinutes"]);
                            var expirationBlack = DateTime.Now.AddMinutes(expirationMinuesBlack);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return (new(), 4);
                }
            }
            else
            {
                return (new(), 5);
            }

            user.Item1.RefreshToken = await GenerateRefreshToken();
            user.Item1.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

            var userUpdate = _mapper.Map<AccountCUDTO>(user.Item1);
            await _accountService.UpdateTokenOnLoginAsync(userUpdate);

            var accessToken = this.CreateToken(user.Item1);
            int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]);
            var expiration = DateTime.Now.AddMinutes(expirationMinutes);

            return (new ApplicationUser
            {
                Token = accessToken,
                RefreshToken = user.Item1.RefreshToken,
                RefreshTokenExpiryTime = expiration,
            }, 0);
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var validReset = await _passwordResetRepository.GetValidTokenAsync(email, token);
            if (validReset == null) return false;

            var account = await _accountRepostiory.GetAccountByEmail(email);
            if (account == null) return false;

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _accountRepostiory.UpdatePasswordAsync(account, hashPassword);

            await _passwordResetRepository.MarkAsUsedAsync(validReset);

            return true;
        }

        public async Task<bool> SendForGotPasswordEmailAsync(string email)
        {
            var user = await _accountRepostiory.GetAccountByEmail(email);
            if(user == null) return false;

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
            var expireAt = DateTime.UtcNow.AddMinutes(15);
            await _passwordResetRepository.AddTokenAsync(email, token, expireAt);
            var link = $"https://localhost:7204/Authentication/ResetPassword?email={email}&token={Uri.EscapeDataString(token)}";

            await SendEmailAsync(email, "Password recovery", $@"
                <p>Hello guy {user.Fullname},</p>
                <p>You have requested to reset your password. Click the link below to continue:</p>
                <a href='{link}'>Reset password</a>
                <p>This link will expire after 15 minutes.</p>
            ");

            return true;
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("truongtranlong23@gmail.com", "njya efpy hyyp retk"),
                EnableSsl = true,
            };

            var mail = new MailMessage("truongtranlong23@gmail.com", to, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);

        }
    }
}
