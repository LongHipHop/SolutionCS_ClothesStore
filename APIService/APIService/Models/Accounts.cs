using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace APIService.Models
{
    public class Accounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Fullname { get; set; }

        [Required, StringLength(500)]
        public string Email { get; set; }
        [Required, StringLength(72)]
        public string Password { get; set; }
        [StringLength(50)]
        public string Phone {  get; set; }
        public int RoleId { get; set; }
        public DateOnly? BirthDay { get; set; }

        public string Gender { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateOnly? CreateAt { get; set; }
        public DateOnly? UpdateAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int TokenVersion { get; set; }

        [StringLength(200)]
        public string? Image {  get; set; }

        public Roles Role { get; set; }

        public ICollection<Carts> Carts { get; set; }
        public ICollection<Orders> Orders { get; set; }
        public ICollection<Reviews> Reviews { get; set; }
    }
}
