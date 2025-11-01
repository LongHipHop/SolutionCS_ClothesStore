using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class ProductVariantRepository : RepositoryBase<ProductVariants>, IProductVariantRepository
    {
        public ProductVariantRepository(ShopDbContext context) : base(context) { }

        public Task CreateProductVariant(ProductVariants productVariants)
        {
            return Create(productVariants);
        }

        public Task DeleteProductVariantAsync(ProductVariants products)
        {
            return Delete(products);
        }

        public Task<List<ProductVariants>> GetAllProductVariant()
        {
            return _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Color)
                .Include(pv => pv.Size)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductVariants?> GetProductVariantsAsync(int productId, int colorId, int sizeId)
        {
            return await _context.ProductVariants
                            .Where(v => v.ProductId == productId && v.ColorId == colorId && v.SizeId == sizeId)
                            .FirstOrDefaultAsync();
        }

        public async Task<ProductVariants> GetProductVariantsById(int productId)
        {
            return await _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Color)
                .Include(pv => pv.Size)
                .AsNoTracking()
                .FirstOrDefaultAsync(pv => pv.Id == productId);
        }

        public Task UpdateProductVariantAsync(ProductVariants productVariants)
        {
            return Update(productVariants);
        }
    }
}
