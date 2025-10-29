using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IPasswordResetRepository
    {
        Task<PasswordResets?> GetValidTokenAsync(string email, string token);
        Task AddTokenAsync(string email, string token, DateTime expireAt);
        Task MarkAsUsedAsync(PasswordResets reset);
    }
}
