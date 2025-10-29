using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIService.Repository.Implementations
{
    public class PaymentRepository : RepositoryBase<Payments>, IPaymentRepository
    {
        public PaymentRepository(ShopDbContext context) : base(context)
        {
        }

        public Task CreatePayment(Payments payment)
        {
            return Create(payment);
        }

        public async Task<IEnumerable<Payments>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(x => x.OrderId == orderId)
                .ToListAsync();
        }
    }
}
