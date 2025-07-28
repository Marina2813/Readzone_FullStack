using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IPostRepository: IGenericRepository<Post>
    {
        //Task AddPostAsync(Post post);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(string postId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<Post>> GetUserPostsAsync(int userId);
        Task<IEnumerable<Post>> SearchPostsAsync(string query);
        //Task SaveChangesAsync();
        //void RemovePost(Post post);
    }
}
