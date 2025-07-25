using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task<Comment?> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Post?> GetPostByIdAsync(string postId)
        {
            return await _context.Posts.FindAsync(postId);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForPostAsync(string postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.Timestamp)
                .ToListAsync();
        }

        public async Task<int> GetCommentCountAsync(string postId)
        {
            return await _context.Comments.CountAsync(c => c.PostId == postId);
        }

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

