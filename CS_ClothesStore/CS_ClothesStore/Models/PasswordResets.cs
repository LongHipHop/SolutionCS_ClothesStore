using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS_ClothesStore.Models
{
    public class PasswordResets
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(500)]
        public string Email { get; set; }

        [Required, StringLength(500)]
        public string Token { get; set; }

        public DateTime ExpireAt { get; set; }

        public bool Used { get; set; } = false;
    }
}
