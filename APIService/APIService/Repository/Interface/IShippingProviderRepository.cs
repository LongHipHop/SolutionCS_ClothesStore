using APIService.Models;
using APIService.Repository.Implementations;

namespace APIService.Repository.Interface
{
    public interface IShippingProviderRepository : IRepositoryBase<ShippingProviders>
    {
        Task<List<ShippingProviders>> GetAll();
    }
}
