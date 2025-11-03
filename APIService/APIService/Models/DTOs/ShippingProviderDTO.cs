namespace APIService.Models.DTOs
{
    public class ShippingProviderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? DefaultFee { get; set; }
        public string? ContactNumber { get; set; }
    }
}
