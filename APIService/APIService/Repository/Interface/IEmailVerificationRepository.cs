using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IEmailVerificationRepository
    {
        Task<EmailVerification?> GetByEmailAsync(string email);
        Task<EmailVerification?> GetByTokenAsync(string token);
        Task AddEmailVerification(EmailVerification emailVerification);
        Task UpdateEmailVerification(EmailVerification emailVerification);
        Task DeleteByEmailAsync(string email);
    }
}
