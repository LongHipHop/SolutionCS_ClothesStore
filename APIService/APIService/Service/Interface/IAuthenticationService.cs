using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<(ApplicationUser, int)> Login(LoginModel loginModel);
        Task<int> Logout(string token);
        Task<string> GenerateRefreshToken();

        string CreateToken(AccountDTO user);
        Task<(ApplicationUser, int)> RefreshToken(RequestTokenModel requestTokenModel, string oldAccessToken);

        Task<bool> SendForGotPasswordEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
