using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface ISizeService
    {
        Task<(List<SizeDTO>, int)> GetAll();
        Task<(SizeDTO, int)> GetSizeById(int id);
        Task<int> CreateSize(SizeCUDTO sizeCUDTO);
        Task<int> UpdateSize(SizeCUDTO sizeCUDTO);
        Task<int> DeleteSize(int id);
    }
}
