using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Post
    {
        [Key]
        public string PostId { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }





    }
}
