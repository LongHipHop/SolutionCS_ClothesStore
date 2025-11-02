using APIService.Models;
using APIService.Repository.Interface;

namespace APIService.Repository.Implementations
{
    public class ShippingProviderRepository : RepositoryBase<ShippingProviders>, IShippingProviderRepository
    {
        public ShippingProviderRepository(ShopDbContext context) : base(context) { }

    }
}
