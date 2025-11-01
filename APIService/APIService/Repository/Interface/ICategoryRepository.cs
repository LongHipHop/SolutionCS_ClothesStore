using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<List<Categories>> GetAll();
        Task CreateCategory(Categories category);
        Task UpdateCategoryAsync(Categories category);
        Task DeleteCategoryAsync(Categories category);
        Task<Categories> GetCategorysById(int categoryId);
    }
}
