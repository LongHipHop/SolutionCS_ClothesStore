using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace APIService.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string ProductName { get; set; }
        [Required, StringLength(500)]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        public double Price { get; set; }
        public double Discount { get; set; }
        [Required, StringLength(200)]
        public string Image {  get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Categories Category { get; set; }
        public ICollection<ProductVariants> ProductVariants { get; set; }
        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<ProductPromotions> ProductPromotions { get; set; }
    }
}
