using APIService.Models;

namespace APIService.Repository.Interface
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payments>> GetPaymentsByOrderIdAsync(int orderId);
        Task CreatePayment(Payments payment);
        Task<List<Payments>> GetAllPaymentUnprocessing();
        Task<List<Payments>> GetAll();

        Task<decimal> GetDailyEarningsAsync(DateTime date);
        Task<decimal> GetWeeklyEarningsAsync(DateTime date);
        Task<decimal> GetMonthlyEarningsAsync(int year, int month);
        Task<decimal> GetYearlyEarningsAsync(int year);
        Task<(decimal TotalRevenue, int PaymentCount)> GetDailyRevenueAndCountAsync(DateTime date);

        Task<(decimal TotalRevenue, int PaymentCount, List<(DateTime Date, decimal Amount)> DailyRevenues)> GetRevenueByRangeAsync(DateTime startDate, DateTime endDate);
    }
}
