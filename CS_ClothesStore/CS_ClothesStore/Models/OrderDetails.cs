using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS_ClothesStore.Models
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantsId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        public double UnitPrice { get; set; }

        public Orders Order { get; set; }
        public ProductVariants ProductVariants { get; set; }
    }
}
