using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IPostRepository Posts { get; }
        public ICommentRepository Comments { get; }
        public ILikeRepository Likes { get; }
        public IAuthRepository Auth { get; }

        public UnitOfWork(AppDbContext context,
                          IPostRepository postRepository,
                          ICommentRepository commentRepository,
                          ILikeRepository likeRepository,
                          IAuthRepository authRepository)
        {
            _context = context;
            Posts = postRepository;
            Comments = commentRepository;
            Likes = likeRepository;
            Auth = authRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
