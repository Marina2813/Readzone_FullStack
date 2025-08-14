using WebApplication1.DTOs;

namespace WebApplication1.Services
{
        public interface ICommentService
        {
            Task<CommentDto> AddCommentAsync(string postId, int userId, CreateCommentDto dto);
            Task<bool> DeleteCommentAsync(int commentId, int userId);
            Task<IEnumerable<CommentDto>> GetCommentsForPostAsync(string postId, int? currentUserId);
            Task<int> GetCommentCount(string postId);
    }

}