using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class ProductVariantRepository : RepositoryBase<ProductVariants>, IProductVariantRepository
    {
        public ProductVariantRepository(ShopDbContext context) : base(context) { }

        public async Task<ProductVariants?> GetProductVariantsAsync(int productId, int colorId, int sizeId)
        {
            return await _context.ProductVariants
                            .Where(v => v.ProductId == productId && v.ColorId == colorId && v.SizeId == sizeId)
                            .FirstOrDefaultAsync();
        }
    }
}
