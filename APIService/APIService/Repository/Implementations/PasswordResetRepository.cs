using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class PasswordResetRepository : RepositoryBase<PasswordResets>, IPasswordResetRepository
    {
        public PasswordResetRepository(ShopDbContext context) : base(context) { }

        public async Task AddTokenAsync(string email, string token, DateTime expireAt)
        {
            var reset = new PasswordResets
            {
                Email = email,
                Token = token,
                ExpireAt = expireAt,
                Used = false
            };
            await _context.PasswordResets.AddAsync(reset);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResets?> GetValidTokenAsync(string email, string token)
        {
            return await _context.PasswordResets
                .Where(r => r.Email == email && r.Token == token && !r.Used && r.ExpireAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task MarkAsUsedAsync(PasswordResets reset)
        {
            reset.Used = true;
            _context.PasswordResets.Update(reset);
            await _context.SaveChangesAsync();
        }
    }
}
