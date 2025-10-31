using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Update;
using Org.BouncyCastle.Crypto.Generators;

namespace APIService.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public AccountService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;

        }
        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<(AccountDTO, int)> GetAccountById(int id)
        {
            try
            {
                var accountExist = await _repositoryManager.AccountRepostiory.GetAccountById(id);
                
                if (accountExist != null)
                {
                    var accountDto = _mapper.Map<AccountDTO>(accountExist);

                    var role = accountExist.Role;
                    if(role != null)
                    {
                        accountDto.RoleName = role.RoleName;
                    }

                    return (accountDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in GetAccountById: {ex.Message}");
                return (new(), 1);
            }
        }

        public async Task<(AccountDTO, int)> GetAccountByEmail(string email)
        {
            try
            {
                var accountExist = await _repositoryManager.AccountRepostiory.GetAccountByEmail(email);

                if (accountExist != null)
                {
                    var accountDto = _mapper.Map<AccountDTO>(accountExist);
                    var role = accountExist.Role;
                    if (role != null)
                    {
                        accountDto.RoleName = role.RoleName;
                    }
                    return (accountDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (new(), 1);
            }
        }

        public async Task<(AccountDTO, int)> GetAccountByLogin(LoginModel loginModel)
        {
            try
            {
                var accountExist = await _repositoryManager.AccountRepostiory.GetAccountByLogin(loginModel);
                if (accountExist != null)
                {
                    if(!BCrypt.Net.BCrypt.Verify(loginModel.Password, accountExist.Password))
                    {
                        return (new(), 4);
                    }
                    var accountDto = _mapper.Map<AccountDTO>(accountExist);
                    if (accountExist.Status == "InActive" || accountExist.Status == "Blocked" || accountExist.Status == "Waiting")
                    {
                        return (new(), 3);
                    }
                    return (accountDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAccountByLogin: {ex.Message}");
                return (new(), 1);
            }
        }

        

        public Task<(AccountDTO, int)> GetAccountByToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<AccountDTO>, int)> GetAll()
        {
            try
            {
                var accountExist = await _repositoryManager.AccountRepostiory.GetAll();
                if(accountExist.Count != 0)
                {
                    var accountDto = _mapper.Map<List<AccountDTO>>(accountExist);

                    for (int i = 0; i < accountDto.Count; i++)
                    {
                        var role = accountExist[i].Role;

                        if (role != null)
                        {
                            accountDto[i].RoleName = role.RoleName;
                        }
                    }
                    return (accountDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in GetAll(): {ex.Message}");
                return (new(), 1);
            }
        }

        public async Task<int> RegisterAsync(AccountCUDTO accountCreate)
        {
            try
            {
                accountCreate.Email = accountCreate.Email.ToLower();

                var account = _mapper.Map<Accounts>(accountCreate);
                account.Fullname = accountCreate.Email;
                account.Phone = accountCreate.Phone;
                account.Address = accountCreate.Address;
                account.BirthDay = accountCreate.BirthDay;
                account.Status = "Active";
                account.CreateAt = DateOnly.FromDateTime(DateTime.Now);
                account.UpdateAt = DateOnly.FromDateTime(DateTime.Now);
                account.Password = BCrypt.Net.BCrypt.HashPassword(accountCreate.Password);
                account.RefreshToken = Guid.NewGuid().ToString();
                account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                account.TokenVersion = 1;

                var accountExist = await _repositoryManager.AccountRepostiory.GetAccountByEmail(account.Email);
                if(accountExist != null)
                {
                    return 2;
                }

                await _repositoryManager.AccountRepostiory.AddAsync(account);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return 1;
            }
        }

        public async Task<int> UpdateAsync(AccountCUDTO accountDTO)
        {
            try
            {
                var accountExist = await _repositoryManager.AccountRepostiory.GetAccountById(accountDTO.Id);
                if (accountExist != null)
                {
                    accountExist.Fullname = accountDTO.Fullname;
                    accountExist.Address = accountDTO.Address;
                    accountExist.UpdateAt = DateOnly.FromDateTime(DateTime.Now);
                    accountExist.Phone = accountDTO.Phone;
                    accountExist.BirthDay = accountDTO.BirthDay;
                    accountExist.Gender = accountDTO.Gender;
                    accountExist.Image = accountDTO.Image;

                    if (accountExist.Password.Trim() != string.Empty)
                    {
                        accountExist.Password = accountDTO.Password;
                    }

                    await _repositoryManager.AccountRepostiory.UpdateAsync(accountExist);
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return 1;
            }
        }

        public async Task<int> UpdateTokenOnLoginAsync(AccountCUDTO accountUpdate)
        {
            try
            {
                var accountExist = await _repositoryManager.AccountRepostiory.GetAccountById(accountUpdate.Id);

                if(accountExist != null)
                {
                    accountExist.TokenVersion = accountUpdate.TokenVersion;
                    accountExist.RefreshToken = accountUpdate.RefreshToken;
                    accountExist.RefreshTokenExpiryTime = accountUpdate.RefreshTokenExpiryTime;
                    accountExist.UpdateAt = DateOnly.FromDateTime(DateTime.Now);

                    await _repositoryManager.AccountRepostiory.UpdateAsync(accountExist);
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        public async Task<Accounts> LoginWithGoogleAsync(GoogleAccountDTO dto)
        {
            var account = await _repositoryManager.AccountRepostiory.GetAccountByEmail(dto.Email);

            if (account == null)
            {
                account = await _repositoryManager.AccountRepostiory.CreateByGoogleAsync(dto);
            }
            else
            {
                account.Fullname = dto.Fullname ?? account.Fullname;
                account.Image = dto.Image ?? account.Image;
                account.UpdateAt = DateOnly.FromDateTime(DateTime.Now);

                await _repositoryManager.AccountRepostiory.UpdateAsync(account);
            }

            return account;

        }

        public async Task<int> UpdatePassword(string oldPassword, string newPassword, int id)
        {
            try
            {
                var accountExit = await _repositoryManager.AccountRepostiory.GetAccountById(id);

                if (accountExit == null)
                {
                    return 2;
                }

                if (!string.IsNullOrEmpty(oldPassword))
                {
                    bool valid = BCrypt.Net.BCrypt.Verify(oldPassword, accountExit.Password);
                    if(!valid)
                    {
                        return 3;
                    }
                }

                accountExit.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _repositoryManager.AccountRepostiory.UpdateAsync(accountExit);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<int> CountAllUsersAsync()
        {
            try
            {
                var accounts = await _repositoryManager.AccountRepostiory.GetAll();

                return accounts.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        public async Task<(int oldUsers, int newUsers)> CountUsersByWeekAsync()
        {
            try
            {
                var accounts = await _repositoryManager.AccountRepostiory.GetAll();
                var now = DateTime.UtcNow;
                var sevenDaysAgo = DateOnly.FromDateTime(now.AddDays(-7));

                int newAccounts = accounts.Count(a => a.CreateAt >= sevenDaysAgo);
                int oldAccounts = accounts.Count - newAccounts;

                return (oldAccounts, newAccounts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (1, 1);
            }
        }
    }
}
