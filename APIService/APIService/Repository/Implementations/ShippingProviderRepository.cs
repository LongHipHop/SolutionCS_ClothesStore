using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class ShippingProviderRepository : RepositoryBase<ShippingProviders>, IShippingProviderRepository
    {
        public ShippingProviderRepository(ShopDbContext context) : base(context) { }

        public Task<List<ShippingProviders>> GetAll()
        {
            return FindAll(trackChanges:false).ToListAsync();
        }
    }
}
