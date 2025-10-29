using APIService.Models;
using APIService.Models.DTOs;

namespace APIService.Repository.Interface
{
    public interface IAccountRepostiory
    {
        Task<List<Accounts>> GetAll();
        Task<Accounts> GetAccountByLogin(LoginModel loginModel);
        Task<Accounts> GetAccountById(int id);
        Task<Accounts> GetAccountByEmail(string email);
        Task AddAsync(Accounts account);
        Task UpdateAsync(Accounts account);
        Task DeleteAsync(Accounts account);

        //Profile

        Task UpdatePasswordAsync(Accounts accounts, string newHashedPassword);
    }
}
