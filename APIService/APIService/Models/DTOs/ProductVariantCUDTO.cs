using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models.DTOs
{
    public class ProductVariantCUDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }

    }
}
