using WebApplication1.validation;

namespace WebApplication1.DTOs
{
    public record LoginDto(
        [RequiredBind] string Email,
        [RequiredBind] string Password
        )
    ;
}
