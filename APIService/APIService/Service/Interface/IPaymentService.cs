using APIService.Models;
using APIService.Models.DTOs;

namespace APIService.Service.Interface
{
    public interface IPaymentService
    {
        Task<(PaymentCUDTO, int)> CreatePaymentAsync(PaymentCUDTO payments);
        Task<(IEnumerable<PaymentCUDTO>, int)> GetPaymentByOrderIdAsync(int orderId);

        Task<object> GetDailyRevenueAndCountAsync();

        //Task<decimal> TotalMonthEarningAsync(int year, int month);

        Task<decimal> GetTodayEarningsAsync();
        Task<decimal> GetWeeklyEarningsAsync();
        Task<decimal> GetMonthlyEarningsAsync(int year, int month);
        Task<decimal> GetYearlyEarningsAsync(int year);
        Task<object> GetAllEarningsSummaryAsync();

        Task<object> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<int> CountPaymentUnProcessing();

    }
}
