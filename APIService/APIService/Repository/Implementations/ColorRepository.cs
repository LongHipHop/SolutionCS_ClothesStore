using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace APIService.Repository.Implementations
{
    public class ColorRepository : RepositoryBase<Colors>, IColorRepository
    {
        public ColorRepository(ShopDbContext context) : base(context) { }

        public Task CreateColor(Colors color)
        {
            return Create(color);
        }

        public Task DeleteColor(Colors color)
        {
            return Delete(color);
        }

        public Task<List<Colors>> GetAll()
        {
            return FindAll(trackChanges: false).ToListAsync();
        }

        public async Task<Colors> GetColorById(int id)
        {
            var item = await FindByCondition(c => c.Id == id, trackChanges: false, includeRole: false);
            if(item == null)
            {
                return null;
            }

            return item;
        }

        public Task UpdateColor(Colors color)
        {
            return Update(color);
        }
    }
}
