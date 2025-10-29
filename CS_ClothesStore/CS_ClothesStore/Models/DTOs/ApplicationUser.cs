using System.Data;

namespace CS_ClothesStore.Models.DTOs
{
    public class ApplicationUser
    {
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
        public int TokenVersion { get; set; } = 1;
        public string Token { get; set; }
        public List<Roles> Roles { get; set; } = new();
    }
}
