using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<Orders> GetOrderById(int id);
        Task UpdateOrder(Orders order);
        Task CreateOrder(Orders order);
    }
}
