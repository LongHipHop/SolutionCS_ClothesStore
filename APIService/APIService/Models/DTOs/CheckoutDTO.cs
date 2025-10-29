﻿namespace APIService.Models.DTOs
{
    public class CheckoutDTO
    {
        public int AccountId { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string Note { get; set; }
        public List<SelectedItemDTO> SelectedItems { get; set; } = new();
    }
}
