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

        public Task BlockOrder(OrderDTO orderDTO)
        {
            throw new NotImplementedException();
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

        public async Task<(List<OrderDTO>, int)> GetAll()
        {
            try
            {
                var orders = await _repositoryManager.OrderRepository.GetAll();

                if(orders.Count != 0)
                {
                    var orderDto = _mapper.Map<List<OrderDTO>>(orders);
                    return (orderDto, 0);
                }
                else
                {
                    return (new(), 2);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new(), 1);
            }
        }

        public async Task<(OrderDTO, int)> GetOrderById(int orderId)
        {
            try
            {
                var order = await _repositoryManager.OrderRepository.GetOrderById(orderId);

                if(order != null)
                {
                    var orderDto = _mapper.Map<OrderDTO>(order);

                    return (orderDto, 0);
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

        public async Task<int> UpdateOrder(OrderDTO orderDTO)
        {
            if(orderDTO == null)
            {
                return 2;
            }

            try
            {
                var order = await _repositoryManager.OrderRepository.GetOrderById(orderDTO.Id);

                if(order == null)
                {
                    return 3;
                }

                order.Status = orderDTO.Status;

                await _repositoryManager.OrderRepository.UpdateOrder(order);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
