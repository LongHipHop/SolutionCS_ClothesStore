namespace APIService.Models.DTOs
{
    public class ShipmentCUDTO
    {
        public int OrderId { get; set; }
        public int ShippingProviderId { get; set; }
        public DateTime ShipDate { get; set; }
        public string TrackingNumber { get; set; }
        public string Status { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
