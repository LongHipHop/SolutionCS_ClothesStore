using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface ICategoryRepository : IRepositoryBase<Categories>
    {
        Task<List<Categories>> GetAll();
        Task CreateCategory(Categories category);
        Task UpdateCategoryAsync(Categories category);
        Task DeleteCategoryAsync(Categories category);
        Task<Categories> GetCategorysById(int categoryId);
    }
}
