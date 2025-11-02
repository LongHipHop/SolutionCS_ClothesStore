using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IShipmentService
    {
        Task<int> CreateShipment(ShipmentCUDTO shipmentCUDTO);
    }
}
