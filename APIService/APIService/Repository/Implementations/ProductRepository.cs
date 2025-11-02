using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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

        public Task<List<Products>> GetAll()
        {
            return _context.Products
                .Include(pv => pv.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _context.Products
                .Include(pv => pv.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(pv => pv.Id == id);
        }

        public Task UpdateProduct(Products product)
        {
            return Update(product);
        }
    }
}
