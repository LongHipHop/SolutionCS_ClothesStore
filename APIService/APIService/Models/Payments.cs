using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        public double Amount { get; set; }
        [StringLength(100)]
        public string PaymentMethod { get; set; }
        [StringLength(20)]
        public string PaymentStatus { get; set; }

        public Orders Order { get; set; }
    }
}
