using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services
{
        public interface IPostService
        {
            Task<PostDto> CreatePostAsync(CreatePostDto createDto, int userId);
            Task<PagedResult<PostDto>> GetAllPostsAsync(PaginationParamsDto pagination);
            Task<PostDto?> GetPostByIdAsync(string postId);
            Task<bool> UpdatePostAsync(string postId, CreatePostDto updatedDto, string userEmail);
            Task<bool> DeletePostAsync(string postId, string userEmail);
            Task<IEnumerable<PostDto>> GetUserPostsAsync(string userEmail);
            Task<IEnumerable<PostDto>> SearchPostsAsync(string query);
        }
}

