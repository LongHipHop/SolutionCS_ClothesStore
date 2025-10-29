using APIService.Models;
using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IPaymentService
    {
        Task<(PaymentCUDTO, int)> CreatePaymentAsync(PaymentCUDTO payments);
        Task<(IEnumerable<PaymentCUDTO>, int)> GetPaymentByOrderIdAsync(int orderId);
    }
}
