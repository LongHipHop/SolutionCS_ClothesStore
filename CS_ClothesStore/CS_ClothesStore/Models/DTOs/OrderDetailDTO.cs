namespace CS_ClothesStore.Models.DTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingProviderName { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
