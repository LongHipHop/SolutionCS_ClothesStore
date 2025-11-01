using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IProductRepository : IRepositoryBase<Products>
    {
        Task<List<Products>> GetAll();
        Task<Products> GetProductById(int id);
        Task AddProduct(Products product);
        Task DeleteProduct(Products product);
        Task UpdateProduct(Products product);
    }
}
