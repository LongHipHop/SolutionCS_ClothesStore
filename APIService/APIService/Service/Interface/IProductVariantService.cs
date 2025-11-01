using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IProductVariantService
    {
        Task<(List<ProductVariantDTO>, int)> GetAll();
        Task<(ProductVariantDTO, int)> GetProductVariantById(int id);
        Task<int> CreateProductVariant(ProductVariantCUDTO productVariantCUDTO);
        Task<int> UpdateProductVariant(ProductVariantCUDTO productVariantCUDTO);
        Task<int> DeleteProductVariant(int id);
    }
}
