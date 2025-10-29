using System.ComponentModel.DataAnnotations;

namespace APIService.Models.DTOs
{
    public class PaymentCUDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }
}
