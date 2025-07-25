using WebApplication1.Models;

namespace WebApplication1.Services
{
        public interface IPostService
        {
            Task<Post> CreatePostAsync(Post post, int userId);
            Task<IEnumerable<Post>> GetAllPostsAsync();
            Task<Post?> GetPostByIdAsync(string postId);
            Task<bool> UpdatePostAsync(string postId, Post updatedPost, string userEmail);
            Task<bool> DeletePostAsync(string postId, string userEmail);
            Task<IEnumerable<object>> GetUserPostsAsync(string userEmail);
            Task<IEnumerable<Post>> SearchPostsAsync(string query);
        }
}

