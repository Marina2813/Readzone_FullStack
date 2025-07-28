// Services/PostService.cs
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;

        public PostService(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<Post> CreatePostAsync(Post post, int userId)
        {
            post.PostId = $"P-{Guid.NewGuid()}";
            post.UserId = userId;
            post.CreatedDate = DateTime.UtcNow;

            await _repository.AddAsync(post);
            await _repository.SaveChangesAsync();

            return post;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _repository.GetAllPostsAsync();
        }

        public async Task<Post?> GetPostByIdAsync(string postId)
        {
            return await _repository.GetPostByIdAsync(postId);
        }

        public async Task<bool> UpdatePostAsync(string postId, Post updatedPost, string userEmail)
        {
            var post = await _repository.GetByIdAsync(postId);
            if (post == null) return false;

            var user = await _repository.GetUserByEmailAsync(userEmail);
            if (user == null || post.UserId != user.Id) return false;

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;

            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(string postId, string userEmail)
        {
            var post = await _repository.GetByIdAsync(postId);
            if (post == null) return false;

            var user = await _repository.GetUserByEmailAsync(userEmail);
            if (user == null || post.UserId != user.Id) return false;

            _repository.Remove(post);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<object>> GetUserPostsAsync(string userEmail)
        {
            var user = await _repository.GetUserByEmailAsync(userEmail);
            if (user == null) return Enumerable.Empty<object>();

            var posts = await _repository.GetUserPostsAsync(user.Id);

            return posts.Select(p => new
            {
                p.PostId,
                p.Title,
                p.Content,
                p.CreatedDate,
                UserId = p.UserId,
                Username = p.User?.Username ?? "Unknown"
            });
        }

        public async Task<IEnumerable<Post>> SearchPostsAsync(string query)
        {
            return await _repository.SearchPostsAsync(query);
        }
    }
}
