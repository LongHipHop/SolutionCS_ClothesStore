using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<List<Categories>> GetAll();

    }
}
