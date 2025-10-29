using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models
{
    public class ProductVariants
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Stock {  get; set; }
        [Column(TypeName ="Decimal(10,2)")]
        public double Price { get; set; }

        public Products Product { get; set; }
        public Colors Color { get; set; }
        public Sizes Size { get; set; }

        public ICollection<CartItems> CartItems { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
