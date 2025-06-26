using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreateCommentDto
    {

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
    }

}
