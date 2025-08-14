using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserDto userDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<string> ResetPasswordAsync(ResetPasswordDto dto);
        Task<string?> GetUsernameByIdAsync(int id);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);

    }
}
