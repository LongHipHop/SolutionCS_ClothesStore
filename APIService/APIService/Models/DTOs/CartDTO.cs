namespace APIService.Models.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemDTO> Items { get; set; } = new();
    }
}
