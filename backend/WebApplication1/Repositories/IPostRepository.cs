using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IPostRepository: IGenericRepository<Post>
    {
        //Task AddPostAsync(Post post);
        // IPostRepository.cs
        Task<PagedResult<Post>> GetAllPostsAsync(PaginationParamsDto pagination);

        Task<Post?> GetPostByIdAsync(string postId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<Post>> GetUserPostsAsync(int userId);
        Task<IEnumerable<Post>> SearchPostsAsync(string query);

        //Task SaveChangesAsync();
        //void RemovePost(Post post);
    }
}
