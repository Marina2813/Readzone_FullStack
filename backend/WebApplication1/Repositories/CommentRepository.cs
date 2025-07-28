using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Comment> _genericRepository;
        public CommentRepository(AppDbContext context, IGenericRepository<Comment> genericRepository)
        {
            _context = context;
            _genericRepository = genericRepository;
        }

        public Task<Comment?> GetByIdAsync(object id)
        {
            return _genericRepository.GetByIdAsync(id);
        }
        public Task AddAsync(Comment entity)
        {
            return _genericRepository.AddAsync(entity);
        }

        public void Remove(Comment entity)
        {
            _genericRepository.Remove(entity);
        }

        public Task SaveChangesAsync()
        {
            return _genericRepository.SaveChangesAsync();
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

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        => await _context.Comments.FindAsync(commentId);

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        public Task<IEnumerable<Comment>> GetAllAsync()
        {
            return _genericRepository.GetAllAsync();
        }


    }

}

