using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class SizeRepostiory : RepositoryBase<Sizes>, ISizeRepository
    {
        public SizeRepostiory(ShopDbContext context) : base(context) { }

        public Task CreateSize(Sizes sizes)
        {
            return Create(sizes);
        }

        public Task DeleteSizeAsync(Sizes size)
        {
            return Delete(size);
        }

        public Task<List<Sizes>> GetAllSize()
        {
            return FindAll(trackChanges:false).ToListAsync();
        }

        public async Task<Sizes> GetSizesById(int sizeId)
        {
            var item = await FindByCondition(s => s.Id == sizeId,trackChanges: false, includeRole: false);
            if(item == null)
            {
                return null;
            }
            return item;
        }

        public Task UpdateSizeAsync(Sizes sizes)
        {
            return Update(sizes);
        }
    }
}
