namespace APIService.Models.DTOs
{
    public class DailyRevenueDTO
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TransactionCount { get; set; }
    }
}
