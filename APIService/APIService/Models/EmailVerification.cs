using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models
{
    public class EmailVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Token { get; set; } = null!;

        public DateTime ExpireAt { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
