using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface ICartItemRepository
    {
        Task<CartItems?> GetCartItemAsync(int cartId, int productVariantId);
        Task UpdateCartItem(CartItems cartItems);
        Task CreateCartItem(CartItems cartItems);
        Task RemoveSelectedItemAsync(List<int> productIds, int accountId);
    }
}
