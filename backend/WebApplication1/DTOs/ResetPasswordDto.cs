namespace WebApplication1.DTOs
{
    public record ResetPasswordDto(
        string Email,
        string NewPassword
    );
    
}
