using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS_ClothesStore.Models
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime OrderDate { get; set; }
        [Column(TypeName ="Decimal(10,2)")]
        public double TotalPrice { get; set; }
        [StringLength(20)]
        public string Status {  get; set; }
        [StringLength(200)]
        public string ShippingAddress { get; set; }
        [StringLength(100)]
        public string PaymentMethod { get; set; }
        [StringLength(500)]
        public string? Note { get; set; }

        public Accounts Account { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
        public ICollection<Payments> Payments { get; set; }
        public ICollection<Shipments> Shipments { get; set; }
    }
}
