using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<List<Orders>> GetAll();
        Task<Orders> GetOrderById(int id);

        Task<Orders> GetOrderDetailById(int id);

        Task UpdateOrder(Orders order);
        Task CreateOrder(Orders order);
    }
}
