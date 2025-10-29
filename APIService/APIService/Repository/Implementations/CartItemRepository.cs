using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class CartItemRepository : RepositoryBase<CartItems>, ICartItemRepository
    {
        public CartItemRepository(ShopDbContext context) : base(context) { }

        

        public Task CreateCartItem(CartItems cartItems)
        {
            return Create(cartItems);
        }

        public Task<CartItems?> GetCartItemAsync(int cartId, int productVariantId)
        {
            return FindByConditionQuery(c => c.CartId == cartId && c.ProductVariantId == productVariantId, false)
                .FirstOrDefaultAsync();
        }

        public async Task RemoveSelectedItemAsync(List<int> productIds, int accountId)
        {
            var itemsToRemove = await _context.CartItems
                .Where(c => c.Cart.AccountId == accountId && productIds.Contains(c.ProductVariantId))
                .ToListAsync();

            if (itemsToRemove.Any())
            {
                _context.CartItems.RemoveRange(itemsToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public Task UpdateCartItem(CartItems cartItems)
        {
            return Update(cartItems);
        }

        
    }
}
