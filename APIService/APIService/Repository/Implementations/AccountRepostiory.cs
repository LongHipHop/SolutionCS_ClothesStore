using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class AccountRepostiory : RepositoryBase<Accounts>, IAccountRepostiory
    {
        public AccountRepostiory(ShopDbContext context) : base(context) { }

        public Task AddAsync(Accounts account)
        {
            return Create(account);
        }

        public Task DeleteAsync(Accounts account)
        {
            return Delete(account);
        }

        public async Task<Accounts> GetAccountByEmail(string email)
        {
            var item = await FindByCondition(a => a.Email == email, trackChanges: true, includeRole: true);
            if(item == null)
            {
                return null;
            }
            return item;
        }

        public async Task<Accounts> GetAccountById(int id)
        {
            var item = await FindByCondition(a => a.Id == id, trackChanges: false, includeRole: true);
            if(item == null)
            {
                return null;
            }
            return item;
        }

        public async Task<Accounts> GetAccountByLogin(LoginModel loginModel)
        {
            return await FindByCondition(a => a.Email == loginModel.Email.ToLower(), 
                trackChanges: false,
                includeRole: false);
        }

        public async Task<List<Accounts>> GetAll()
        {
            return await FindAll(false, query => query.Include(a => a.Role)).ToListAsync();
        }

        public Task UpdateAsync(Accounts account)
        {
            return Update(account);
        }

        public async Task UpdatePasswordAsync(Accounts accounts, string newHashedPassword)
        {
            accounts.Password = newHashedPassword;
            accounts.UpdateAt = DateOnly.FromDateTime(DateTime.Now);
            _context.Accounts.Update(accounts);
            await _context.SaveChangesAsync();
        }
    }
}
