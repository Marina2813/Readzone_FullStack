using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ILikeRepository
    {
        Task<Like?> GetLikeAsync(int userId, string postId);
        //Task AddLikeAsync(Like like);
        //Task RemoveLikeAsync(Like like);
        Task<int> GetLikeCountAsync(string postId);
        Task<bool> HasUserLikedPostAsync(int userId, string postId);
        Task<User?> GetUserByEmailAsync(string email);

        Task AddAsync(Like like);
        void Remove(Like like);
        Task SaveChangesAsync();
    }
}
