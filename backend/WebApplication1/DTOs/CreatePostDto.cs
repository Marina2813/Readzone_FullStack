using WebApplication1.validation;

namespace WebApplication1.DTOs
{
    public record CreatePostDto(
    [RequiredBind] string Title,
    [RequiredBind] string Content,
    string Category
);
}
