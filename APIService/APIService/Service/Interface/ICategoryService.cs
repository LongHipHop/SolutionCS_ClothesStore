using APIService.Models;
using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface ICategoryService
    {
        Task<(List<CategoryDTO>, int)> GetAll();
    }
}
