using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IShippingProviderService
    {
        Task<(List<ShippingProviderDTO>, int)> GetAll();
    }
}
