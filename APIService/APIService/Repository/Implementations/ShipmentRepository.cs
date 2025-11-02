using APIService.Models;
using APIService.Repository.Interface;

namespace APIService.Repository.Implementations
{
    public class ShipmentRepository : RepositoryBase<Shipments>, IShipmentRepository
    {
        public ShipmentRepository(ShopDbContext context) : base(context) { }

        public Task AddShipment(Shipments shipment)
        {
            return Create(shipment);
        }
    }
}
