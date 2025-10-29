namespace CS_ClothesStore.Models.DTOs
{
    public class CheckoutDTO
    {
        public int AccountId { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string Note { get; set; }

        public List<CartItemDTO> SelectedItems { get; set; }
    }
}
