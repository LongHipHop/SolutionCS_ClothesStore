using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface ICartRepository
    {
        Task<Carts> GetCartByAccountIdAsync(int accountId);
        Task CreateCart(Carts carts);
        Task<Carts?> GetCartWithItemsByAccountIdAsync(int accountId);

        Task<Carts> GetCartById(int id);

        Task ClearCartAsync(int accountId);
    }
}
