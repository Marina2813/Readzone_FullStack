using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        //private readonly AppDbContext _context;
        private readonly DbSet<User> _users;

        public PostRepository(AppDbContext context): base(context)
        {
            _users = context.Users;
        }

        /*public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
        }*/

        public async Task<PagedResult<Post>> GetAllPostsAsync(PaginationParamsDto pagination)
        {
            var query = _context.Posts.Include(p => p.User).OrderByDescending(p => p.CreatedDate);
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.ValidatedPageSize)
                .Take(pagination.ValidatedPageSize)
                .ToListAsync();

            return new PagedResult<Post>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.ValidatedPageSize
            };
        }


        public async Task<Post?> GetPostByIdAsync(string postId)
        {
            return await _context.Posts.Include(p => p.User)
                                       .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Post>> GetUserPostsAsync(int userId)
        {
            return await _context.Posts.Where(p => p.UserId == userId)
                                       .Include(p => p.User)
                                       .OrderByDescending(p => p.CreatedDate)
                                       .ToListAsync();
        }

        public async Task<IEnumerable<Post>> SearchPostsAsync(string query)
        {
            var lowerQuery = query.ToLower();
            return await _context.Posts.Where(p => p.Title.ToLower().Contains(lowerQuery) ||
                                                   p.Content.ToLower().Contains(lowerQuery))
                                       .Include(p => p.User)
                                       .OrderByDescending(p => p.CreatedDate)
                                       .ToListAsync();
        }

        /*public void RemovePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }*/
    }
}
