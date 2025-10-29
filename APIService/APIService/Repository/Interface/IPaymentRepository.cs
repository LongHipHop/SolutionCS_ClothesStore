using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payments>> GetPaymentsByOrderIdAsync(int orderId);
        Task CreatePayment(Payments payment);
    }
}
