using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}
