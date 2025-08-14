using System.ComponentModel.DataAnnotations;
using WebApplication1.validation;

namespace WebApplication1.DTOs
{
    public record CreateCommentDto(
        [RequiredBind]
        [MaxLength(1000)]
        string Content
    );

}
