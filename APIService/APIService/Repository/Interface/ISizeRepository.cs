using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface ISizeRepository : IRepositoryBase<Sizes>
    {
        Task CreateSize(Sizes sizes);
        Task UpdateSizeAsync(Sizes sizes);
        Task DeleteSizeAsync(Sizes size);
        Task<List<Sizes>> GetAllSize();
        Task<Sizes> GetSizesById(int sizeId);
    }
}
