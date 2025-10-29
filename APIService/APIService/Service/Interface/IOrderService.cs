using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IOrderService
    {
        Task<(int orderId, int code)> CreateOrder(OrderDTO orderDTO);
    }
}
