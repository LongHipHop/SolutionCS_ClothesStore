using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models
{
    public class Shipments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ShippingProviderId { get; set; }
        public DateTime ShipDate { get; set; }
        [StringLength(20)]
        public string TrackingNumber { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Orders Order { get; set; }
        public ShippingProviders ShippingProvider { get; set; }
    }
}
