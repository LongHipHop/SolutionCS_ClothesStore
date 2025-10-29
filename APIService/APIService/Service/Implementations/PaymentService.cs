using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Interface;
using APIService.Service.Interface;

namespace APIService.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryManager _repositoryManager;

        public PaymentService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<(PaymentCUDTO, int)> CreatePaymentAsync(PaymentCUDTO dto)
        {
            var order = await _repositoryManager.OrderRepository.GetOrderById(dto.OrderId);
            if(order == null)
            {
                return (new(), 1);
            }

            var payment = new Payments
            {
                OrderId = dto.OrderId,
                PaymentDate = dto.PaymentDate,
                Amount = order.TotalPrice,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentStatus,
            };

            order.Status = "Paid";

            await _repositoryManager.PaymentRepository.CreatePayment(payment);

            await _repositoryManager.OrderRepository.UpdateOrder(order);

            var resultDto = new PaymentCUDTO
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
            };

            return (resultDto, 0);

        }

        public async Task<(IEnumerable<PaymentCUDTO>, int)> GetPaymentByOrderIdAsync(int orderId)
        {
            var item = await _repositoryManager.PaymentRepository.GetPaymentsByOrderIdAsync(orderId);
            if(item == null)
            {
                return (null, 1);
            }

            var result = item.Select(p => new PaymentCUDTO
            {
                Id = p.Id,
                OrderId = p.OrderId,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                PaymentStatus = p.PaymentStatus,
            });

            return (result, 0);
        }
    }
}
