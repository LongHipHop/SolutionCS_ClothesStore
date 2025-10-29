using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IProductVariantRepository
    {
        Task<ProductVariants?> GetProductVariantsAsync(int productId, int colorId, int sizeId);
    }
}
