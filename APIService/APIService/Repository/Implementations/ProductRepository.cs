using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class ProductRepository : RepositoryBase<Products>,IProductRepository
    {
        public ProductRepository(ShopDbContext context) : base(context) { }

        public Task AddProduct(Products product)
        {
            return Create(product);
        }

        public Task DeleteProduct(Products product)
        {
            return Delete(product);
        }

        public async Task<List<Products>> GetAll()
        {
            return await FindAll(trackChanges: false).ToListAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            var item = await FindByCondition(a => a.Id == id, trackChanges: false, includeRole: false);
            if(item == null)
            {
                return null;
            }

            return item;
        }

        public Task UpdateProduct(Products product)
        {
            return Update(product);
        }
    }
}
