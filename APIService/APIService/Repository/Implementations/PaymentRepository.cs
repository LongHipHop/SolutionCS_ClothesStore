using APIService.Models;
using APIService.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

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

        public async Task<decimal> GetDailyEarningsAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDat = startOfDay.AddDays(1);

            var total = await _context.Payments
                .Where(p => p.PaymentDate >= startOfDay && p.PaymentDate < endOfDat && p.PaymentStatus == "Success")
                .SumAsync(p => (decimal)p.Amount);
            return total;
        }

        public async Task<(decimal TotalRevenue, int PaymentCount)> GetDailyRevenueAndCountAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var payments = await _context.Payments
                .Where(p => p.PaymentDate >= startOfDay
                         && p.PaymentDate < endOfDay
                         && p.PaymentStatus == "Success")
                .ToListAsync();

            var totalRevenue = payments.Sum(p => (decimal)p.Amount);
            var count = payments.Count;

            return (totalRevenue, count);
        }

        public Task<decimal> GetMonthlyEarningsAsync(int year, int month)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var total = _context.Payments
                .Where(p => p.PaymentDate >= startOfMonth && p.PaymentDate < endOfMonth && p.PaymentStatus == "Success")
                .SumAsync(p => (decimal)p.Amount);
            return total;
        }

        public async Task<IEnumerable<Payments>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(x => x.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<(decimal TotalRevenue, int PaymentCount, List<(DateTime Date, decimal Amount)> DailyRevenues)> GetRevenueByRangeAsync(DateTime startDate, DateTime endDate)
        {
            var payments = await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate.Date.AddDays(1) && p.PaymentStatus == "Success")
                .ToListAsync();

            var totalRevenue = payments.Sum(p => (decimal)p.Amount);
            var count = payments.Count;

            var dailyRevenues = payments
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => (Date: g.Key, Amount: g.Sum(p => (decimal)p.Amount)))
                .OrderBy(x => x.Date)
                .ToList();

            return (totalRevenue, count, dailyRevenues);
        }

        public async Task<decimal> GetWeeklyEarningsAsync(DateTime date)
        {
            var diff = date.DayOfWeek - DayOfWeek.Monday;
            if(diff < 0)
            {
                diff += 7;
            }
            var startOfWeek = date.AddDays(-diff).Date;
            var endOfWeek = startOfWeek.AddDays(7);

            var total = await _context.Payments
                .Where(p => p.PaymentDate >= startOfWeek && p.PaymentDate < endOfWeek && p.PaymentStatus == "Success")
                .SumAsync(p => (decimal)p.Amount);

            return total;
        }

        public Task<decimal> GetYearlyEarningsAsync(int year)
        {
            var startOfYear = new DateTime(year, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var total = _context.Payments
                .Where(p => p.PaymentDate >= startOfYear && p.PaymentDate < endOfYear && p.PaymentStatus == "Success")
                .SumAsync (p => (decimal)p.Amount);
            return total;
        }
    }
}
