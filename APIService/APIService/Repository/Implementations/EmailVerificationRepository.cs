using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class EmailVerificationRepository : RepositoryBase<EmailVerification>, IEmailVerificationRepository
    {
        public EmailVerificationRepository(ShopDbContext context) : base(context)
        {
        }

        public Task AddEmailVerification(EmailVerification emailVerification)
        {
           return Create(emailVerification);
        }

        public async Task DeleteByEmailAsync(string email)
        {
            var records = await _context.EmailVerification
                .Where(e => e.Email == email && !e.IsUsed)
                .ToListAsync();

            if (records.Any())
            {
                _context.EmailVerification.RemoveRange(records);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<EmailVerification?> GetByEmailAsync(string email)
        {
            return await _context.EmailVerification.FirstOrDefaultAsync(e => e.Email == email && !e.IsUsed);
        }

        public Task<EmailVerification?> GetByTokenAsync(string token)
        {
            return _context.EmailVerification.FirstOrDefaultAsync(e => e.Token == token);
        }

        public Task UpdateEmailVerification(EmailVerification emailVerification)
        {
            return Update(emailVerification);
        }
    }
}
