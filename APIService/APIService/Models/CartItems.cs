namespace APIService.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }

        public Carts Cart { get; set; }
        public ProductVariants ProductVariant { get; set; }
    }
}
