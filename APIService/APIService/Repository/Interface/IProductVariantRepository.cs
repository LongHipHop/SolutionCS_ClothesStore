using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IProductVariantRepository
    {
        Task<ProductVariants?> GetProductVariantsAsync(int productId, int colorId, int sizeId);
        Task CreateProductVariant(ProductVariants productVariants);
        Task UpdateProductVariantAsync(ProductVariants productVariants);
        Task DeleteProductVariantAsync(ProductVariants products);
        Task<List<ProductVariants>> GetAllProductVariant();
        Task<ProductVariants> GetProductVariantsById(int productId);
    }
}
