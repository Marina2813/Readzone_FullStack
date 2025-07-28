using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class CommentService: ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<object> AddCommentAsync(string postId, int userId, string content)
        {
            var post = await _commentRepository.GetPostByIdAsync(postId);
            if (post == null) throw new Exception("Post not found");

            var user = await _commentRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var comment = new Comment
            {
                Name = user.Username,
                Content = content,
                Timestamp = DateTime.UtcNow,
                PostId = postId,
                UserId = userId
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return new
            {
                comment.CommentId,
                comment.Name,
                comment.Content,
                comment.Timestamp
            };
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null) return false;

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("You are not allowed to delete this comment.");

            var deleted = await _commentRepository.DeleteCommentAsync(comment);
            return deleted;

       
        }

        public async Task<IEnumerable<object>> GetCommentsForPostAsync(string postId, int? currentUserId)
        {
            var comments = await _commentRepository.GetCommentsForPostAsync(postId);

            return comments.Select(c => new
            {
                c.CommentId,
                c.Name,
                c.Content,
                Timestamp = c.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                IsOwner = currentUserId != null && c.UserId == currentUserId
            });
        }

        public async Task<int> GetCommentCount(string postId)
        {
            return await _commentRepository.GetCommentCountAsync(postId);
        }
    }
}

