namespace WebApplication1.Services
{
        public interface ICommentService
        {
            Task<object> AddCommentAsync(string postId, int userId, string content);
            Task<bool> DeleteCommentAsync(int commentId, int userId);
            Task<IEnumerable<object>> GetCommentsForPostAsync(string postId, int? currentUserId);
            Task<int> GetCommentCount(string postId);
    }

}