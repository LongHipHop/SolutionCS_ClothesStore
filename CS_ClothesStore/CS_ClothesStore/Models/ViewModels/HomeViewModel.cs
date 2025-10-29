using CS_ClothesStore.Models.DTOs;

namespace CS_ClothesStore.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<ProductDTO> Products { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public List<ProductDTO> ProductDetails { get; set; } = new();

    }
}
