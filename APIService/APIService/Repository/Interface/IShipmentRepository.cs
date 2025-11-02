using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IShipmentRepository
    {
        Task AddShipment(Shipments shipment);
    }
}
