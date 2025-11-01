using APIService.Models;
using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface ICategoryService
    {
        Task<(List<CategoryDTO>, int)> GetAll();
        Task<(CategoryDTO, int)> GetCategoryById(int id);
        Task<int> CreateCategory(CategoryCUDTO categoryCUDTO);
        Task<int> UpdateCategory(CategoryCUDTO categoryCUDTO);
        Task<int> DeleteCategory(int id);
    }
}
