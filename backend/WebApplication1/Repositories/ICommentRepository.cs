using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ICommentRepository: IGenericRepository<Comment>
    {
        //Task<Comment?> GetCommentByIdAsync(int commentId);
        //Task<Comment?> AddCommentAsync(Comment comment);
        Task<Post?> GetPostByIdAsync(string postId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<Comment>> GetCommentsForPostAsync(string postId);
        Task<int> GetCommentCountAsync(string postId);
        Task<Comment?> GetCommentByIdAsync(int commentId);
        Task<bool> DeleteCommentAsync(Comment comment);
        //Task<bool> DeleteCommentAsync(Comment comment);
    }
}
