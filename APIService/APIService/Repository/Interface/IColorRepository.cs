using APIService.Models;
using System.Drawing;

namespace APIService.Repository.Interface
{
    public interface IColorRepository : IRepositoryBase<Colors>
    {
        Task<List<Colors>> GetAll();
        Task<Colors> GetColorById(int id);
        Task CreateColor(Colors color);
        Task DeleteColor(Colors color);
        Task UpdateColor(Colors color);
    }
}
