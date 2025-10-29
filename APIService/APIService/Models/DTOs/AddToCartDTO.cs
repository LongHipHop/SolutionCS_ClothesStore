namespace APIService.Models.DTOs
{
    public class AddToCartDTO
    {
        public int AccountId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
    }
}
