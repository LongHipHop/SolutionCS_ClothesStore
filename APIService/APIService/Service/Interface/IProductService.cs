using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IProductService
    {
        Task<(List<ProductDTO>, int)> GetAll();
        Task<(ProductDTO, int)> GetProductById(int id);
        Task<int> CreateProduct(ProductCUDTO productDTO);
        Task<int> UpdateProduct(ProductCUDTO productDTO);
        Task<int> DeleteProduct(int id);

        Task<int> CountAllProductAsync();
    }
}
