// Services/PostService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostDto createDto, int userId)
        {
            var post = _mapper.Map<Post>(createDto);
            post.PostId = $"P-{Guid.NewGuid()}";
            post.UserId = userId;
            post.CreatedDate = DateTime.UtcNow;


            await _unitOfWork.Posts.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();

            //await _repository.LoadUserAsync(post);
            var postWithUser = await _unitOfWork.Posts.GetPostByIdAsync(post.PostId);
            return _mapper.Map<PostDto>(postWithUser!);
        }

        public async Task<PagedResult<PostDto>> GetAllPostsAsync(PaginationParamsDto pagination)
        {
            var pagedPosts = await _unitOfWork.Posts.GetAllPostsAsync(pagination);

            var postDtos = _mapper.Map<IEnumerable<PostDto>>(pagedPosts.Items);

            return new PagedResult<PostDto>
            {
                Items = postDtos,
                TotalCount = pagedPosts.TotalCount,
                PageNumber = pagedPosts.PageNumber,
                PageSize = pagedPosts.PageSize
            };
        }


        public async Task<PostDto?> GetPostByIdAsync(string postId)
        {
            var post = await _unitOfWork.Posts.GetPostByIdAsync(postId);

            //Console.WriteLine($"[DEBUG] Post: {post?.PostId}, User: {post?.User?.Username}");

            return post != null ? _mapper.Map<PostDto>(post) : null;
        }

        public async Task<bool> UpdatePostAsync(string postId, CreatePostDto updatedDto, string userEmail)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);
            if (post == null) return false;

            var user = await _unitOfWork.Auth.GetUserByEmailAsync(userEmail);
            if (user == null || post.UserId != user.Id) return false;

            post.Title = updatedDto.Title;
            post.Content = updatedDto.Content;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(string postId, string userEmail)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);
            if (post == null) return false;

            var user = await _unitOfWork.Auth.GetUserByEmailAsync(userEmail);
            if (user == null || post.UserId != user.Id) return false;

            _unitOfWork.Posts.Remove(post);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PostDto>> GetUserPostsAsync(string userEmail)
        {
            var user = await _unitOfWork.Auth.GetUserByEmailAsync(userEmail);
            if (user == null) return Enumerable.Empty<PostDto>();

            var posts = await _unitOfWork.Posts.GetUserPostsAsync(user.Id);
            
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<IEnumerable<PostDto>> SearchPostsAsync(string query)
        {
            var posts = await _unitOfWork.Posts.SearchPostsAsync(query);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}
