using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }        
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string? Note { get; set; }
    }
}
