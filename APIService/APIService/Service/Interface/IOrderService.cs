using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IOrderService
    {
        Task<(int orderId, int code)> CreateOrder(OrderDTO orderDTO);
        Task<(OrderDTO, int)> GetOrderById(int orderId);
        Task<(List<OrderDTO>, int)> GetAll();

        Task<int> UpdateOrder(OrderDTO orderDTO);
        Task BlockOrder(OrderDTO orderDTO);
    }
}
