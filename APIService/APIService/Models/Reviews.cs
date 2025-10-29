using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIService.Models
{
    public class Reviews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        [StringLength(300)]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Accounts Accounts { get; set; }
        public Products Products { get; set; }
    }
}
