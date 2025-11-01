using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IColorService
    {
        Task<(List<ColorDTO>, int)> GetAll();
        Task<(ColorDTO, int)> GetColorById(int id);
        Task<int> CreateColor(ColorCUDTO colorCUDTO);
        Task<int> UpdateColor(ColorCUDTO colorCUDTO);
        Task<int> DeleteColor(int id);
    }
}
