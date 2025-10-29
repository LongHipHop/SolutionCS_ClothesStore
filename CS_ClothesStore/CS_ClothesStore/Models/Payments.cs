using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS_ClothesStore.Models
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        [StringLength(100)]
        public string PaymentMethod { get; set; }
        [StringLength(20)]
        public string PaymentStatus { get; set; }

        public Orders Order { get; set; }
    }
}
