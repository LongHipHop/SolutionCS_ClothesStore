using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;
using AutoMapper;
using Azure.Core;

namespace APIService.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public OrderService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<(int orderId, int code)> CreateOrder(OrderDTO orderDTO)
        {
            if(orderDTO == null)
            {
                return (0, 1);
            }

            try
            {
                var order = _mapper.Map<Orders>(orderDTO);

                order.OrderDate = DateTime.Now;
                order.PaymentMethod ??= "Cash";
                order.ShippingAddress ??= "No address provided";
                order.Note ??= "";
                await _repositoryManager.OrderRepository.CreateOrder(order);

                return (order.Id, 0);
            }
            catch (AutoMapperMappingException)
            {
                return (0, 3);
            }
            catch (Exception ex)
            {
                return (0, 2);
            }
        }
    }
}
