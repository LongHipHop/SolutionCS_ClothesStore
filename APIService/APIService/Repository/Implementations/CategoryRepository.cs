using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class CategoryRepository : RepositoryBase<Categories>,ICategoryRepository
    {
        public CategoryRepository(ShopDbContext context): base(context) { }
        public async Task<List<Categories>> GetAll()
        {
            return await FindAll(trackChanges:false).ToListAsync();
        }
    }
}
