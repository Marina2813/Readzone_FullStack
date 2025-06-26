using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public string PostId { get; set; } = string.Empty;
        [ForeignKey("PostId")]
        public Post? Post { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    }

}
