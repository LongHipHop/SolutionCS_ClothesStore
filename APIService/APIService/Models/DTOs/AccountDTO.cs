using System.ComponentModel.DataAnnotations;

namespace APIService.Models.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public DateOnly? BirthDay { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }

        public DateOnly? CreateAt { get; set; }
        public DateOnly? UpdateAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int TokenVersion { get; set; }
        public string? Image { get; set; }
    }
}
