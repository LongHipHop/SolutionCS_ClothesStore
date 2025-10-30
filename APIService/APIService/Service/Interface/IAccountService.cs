using APIService.Models;
using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IAccountService
    {
        Task<(List<AccountDTO>, int)> GetAll();
        Task<(AccountDTO, int)> GetAccountById(int id);
        Task<(AccountDTO, int)> GetAccountByEmail(string email);
        Task<(AccountDTO, int)> GetAccountByLogin(LoginModel loginModel);
        Task<int> UpdateAsync(AccountCUDTO accountDTO);
        Task<int> UpdatePassword(string oldPassword, string newPassword, int id);
        Task<int> DeleteAsync(int id);
        Task<int> RegisterAsync(AccountCUDTO accountCreate);
        Task<(AccountDTO, int)> GetAccountByToken(string token);
        Task<int> UpdateTokenOnLoginAsync(AccountCUDTO accountUpdate);

        Task<Accounts> LoginWithGoogleAsync(GoogleAccountDTO dto);


        //Dashboard Admin

        Task<int> CountAllUsersAsync();
        Task<(int oldUsers, int newUsers)> CountUsersByWeekAsync();
    }
}
