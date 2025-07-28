namespace WebApplication1.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
        ILikeRepository Likes { get; }
        IAuthRepository Auth { get; }

        Task<int> SaveChangesAsync();
    }
}
