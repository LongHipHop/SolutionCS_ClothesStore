using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class CategoryRepository : RepositoryBase<Categories>,ICategoryRepository
    {
        public CategoryRepository(ShopDbContext context): base(context) { }

        public Task CreateCategory(Categories category)
        {
            return Create(category);
        }

        public Task DeleteCategoryAsync(Categories category)
        {
            return Delete(category);
        }

        public async Task<List<Categories>> GetAll()
        {
            return await FindAll(trackChanges:false).ToListAsync();
        }

        public async Task<Categories> GetCategorysById(int categoryId)
        {
            var item = await FindByCondition(c => c.Id == categoryId, trackChanges: false, includeRole: false);
            if(item == null)
            {
                return null;
            }

            return item;
        }

        public Task UpdateCategoryAsync(Categories category)
        {
            return Update(category);
        }
    }
}
