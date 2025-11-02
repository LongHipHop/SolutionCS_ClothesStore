using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models
{
    public class ShippingProviders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? DefaultFee { get; set; }

        [StringLength(100)]
        public string? ContactNumber { get; set; }

        public ICollection<Shipments> Shipments { get; set; }
    }
}
