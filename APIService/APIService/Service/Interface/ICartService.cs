using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface ICartService
    {
        Task<int> AddToCartAsync(int accountId, int productId, int colorId, int sizeId, int quantity);
        Task<CartDTO?> GetCartByAccountIdAsync(int accountId);

        Task<int> ClearCartAsync(int accountId);

        Task RemoveSelectedItemsAsync(List<int> productIds, int accountId);
    }
}
