namespace CS_ClothesStore.Models.DTOs
{
    public class ProductVariantDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
    }
}
