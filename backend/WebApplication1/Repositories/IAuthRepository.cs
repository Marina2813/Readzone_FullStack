using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IAuthRepository: IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);

    }
}
