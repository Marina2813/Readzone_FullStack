using WebApplication1.validation;

namespace WebApplication1.DTOs
{
    public record ResetPasswordDto(
        [RequiredBind] string Email,
        [RequiredBind] string NewPassword
    );
    
}
