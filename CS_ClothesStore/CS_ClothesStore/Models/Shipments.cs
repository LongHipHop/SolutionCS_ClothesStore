using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS_ClothesStore.Models
{
    public class Shipments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ShipDate { get; set; }
        [StringLength(100)]
        public string Carrier { get; set; }
        [StringLength(20)]
        public string TrackingNumber { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Orders Orders { get; set; }
    }
}
