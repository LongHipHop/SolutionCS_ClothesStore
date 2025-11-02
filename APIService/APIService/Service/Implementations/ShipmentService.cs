using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;

namespace APIService.Service.Implementations
{
    public class ShipmentService : IShipmentService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ShipmentService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<int> CreateShipment(ShipmentCUDTO shipmentCUDTO)
        {
            if(shipmentCUDTO == null)
            {
                return 1;
            }

            try
            {

                var shipment = _mapper.Map<Shipments>(shipmentCUDTO);

                await _repositoryManager.ShipmentRepository.AddShipment(shipment);

                return 0;
            }
            catch (AutoMapperMappingException)
            {
                return 4;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 2;
            }
        }
    }
}
