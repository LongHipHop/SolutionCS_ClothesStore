using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;

namespace APIService.Service.Implementations
{
    public class ShippingProviderService : IShippingProviderService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ShippingProviderService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<(List<ShippingProviderDTO>, int)> GetAll()
        {
            try
            {
                var shipping = await _repositoryManager.ShippingProviderRepository.GetAll();

                if(shipping.Count != 0)
                {
                    var shippingDto = _mapper.Map<List<ShippingProviderDTO>>(shipping);

                    return (shippingDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new(), 1);
            }
        }
    }
}
