using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class CartRepository : RepositoryBase<Carts>, ICartRepository
    {
        public CartRepository(ShopDbContext context) : base(context)
        {
        }

        public async Task ClearCartAsync(int accountId)
        {
            var carts = await _context.Carts.Where(c => c.AccountId == accountId).ToListAsync();
            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();
        }

        public Task CreateCart(Carts carts)
        {
            return Create(carts);
        }

        public async Task<Carts?> GetCartByAccountIdAsync(int accountId)
        {
            return await FindByConditionQuery(c => c.AccountId == accountId, trackChanges: false)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();
        }

        public async Task<Carts> GetCartById(int id)
        {
            var item = await FindByCondition(o => o.Id == id, trackChanges: false, includeRole: false);

            if(item == null)
            {
                return null;
            }
            return item;
        }

        public async Task<Carts?> GetCartWithItemsByAccountIdAsync(int accountId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.ProductVariant)
                .ThenInclude(pv => pv.Product)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.ProductVariant.Color)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.ProductVariant.Size)
                .FirstOrDefaultAsync(c => c.AccountId == accountId);
        }
    }
}
