using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace APIService.Models.DTOs
{
    public class GoogleAccountDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string? Image {  get; set; }
    }
}
