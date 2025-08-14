using WebApplication1.validation;

namespace WebApplication1.DTOs
{
    public record UserDto(
        [RequiredBind] string Email,
        [RequiredBind] string Password,
        [RequiredBind] string Username);
    
}
