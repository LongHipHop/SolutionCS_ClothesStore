namespace CS_ClothesStore.Models
{
    public class ProductPromotions
    {
        public int ProductId { get; set; }
        public int PromotionId { get; set; }
        public Products Products { get; set; }
        public Promotions Promotions { get; set; }
    }
}
