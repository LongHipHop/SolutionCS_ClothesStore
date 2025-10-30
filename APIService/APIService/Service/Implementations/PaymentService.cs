using APIService.Models;
using APIService.Models.DTOs;
using APIService.Repository.Implementations;
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

        public async Task<decimal> GetTodayEarningsAsync()
        {
            return await _repositoryManager.PaymentRepository.GetDailyEarningsAsync(DateTime.Now);
        }

        public async Task<decimal> GetWeeklyEarningsAsync()
        => await _repositoryManager.PaymentRepository.GetWeeklyEarningsAsync(DateTime.Now);

        public async Task<decimal> GetMonthlyEarningsAsync(int year, int month)
            => await _repositoryManager.PaymentRepository.GetMonthlyEarningsAsync(year, month);

        public async Task<decimal> GetYearlyEarningsAsync(int year)
            => await _repositoryManager.PaymentRepository.GetYearlyEarningsAsync(year);

        public async Task<object> GetAllEarningsSummaryAsync()
        {
            var now = DateTime.Now;

            var daily = await _repositoryManager.PaymentRepository.GetDailyEarningsAsync(now);
            var weekly = await _repositoryManager.PaymentRepository.GetWeeklyEarningsAsync(now);
            var monthly = await _repositoryManager.PaymentRepository.GetMonthlyEarningsAsync(now.Year, now.Month);
            var yearly = await _repositoryManager.PaymentRepository.GetYearlyEarningsAsync(now.Year);

            return new
            {
                Daily = daily,
                Weekly = weekly,
                Monthly = monthly,
                Yearly = yearly
            };
        }

        public async Task<object> GetDailyRevenueAndCountAsync()
        {
            var today = DateTime.Now;
            var (totalRevenue, count) = await _repositoryManager.PaymentRepository.GetDailyRevenueAndCountAsync(today);

            return new
            {
                Date = today.ToString("dd-MM-yyyy"),
                TotalRevenue = totalRevenue,
                PaymentCount = count
            };
        }

        public async Task<object> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var (totalRevenue, count, dailyRevenues) = await _repositoryManager.PaymentRepository.GetRevenueByRangeAsync(startDate, endDate);

            var dailyList = dailyRevenues
                .Select(x => new {date = x.Date.ToString("dd-MM-yyyy"), amount = x.Amount})
                .ToList();

            return new
            {
                StartDate = startDate.ToString("dd-MM-yyyy"),
                EndDate = endDate.ToString("dd-MM-yyyy"),
                totalRevenue = totalRevenue,
                PaymentCount = count,
                dailyRevenues = dailyList
            };
        }
    }
}
