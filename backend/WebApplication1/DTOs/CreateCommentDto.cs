using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public record CreateCommentDto(
        [Required]
        [MaxLength(1000)]
        string Content
    );

}
