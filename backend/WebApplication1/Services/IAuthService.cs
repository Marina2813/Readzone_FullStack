using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserDto userDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
