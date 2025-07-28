using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class LikeRepository: ILikeRepository
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Like> _genericRepository;

        public LikeRepository(AppDbContext context, IGenericRepository<Like> genericRepository) 
        {
            _context = context;
            _genericRepository = genericRepository;
        }

        public async Task<Like?> GetLikeAsync(int userId, string postId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        }

        /* async Task AddLikeAsync(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(Like like)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }
        */
        public async Task<int> GetLikeCountAsync(string postId)
        {
            return await _context.Likes.CountAsync(l => l.PostId == postId);
        }

        public async Task<bool> HasUserLikedPostAsync(int userId, string postId)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == userId && l.PostId == postId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task AddAsync(Like like) => _genericRepository.AddAsync(like);
        public void Remove(Like like) => _genericRepository.Remove(like);
        public Task SaveChangesAsync() => _genericRepository.SaveChangesAsync();
    }
}
