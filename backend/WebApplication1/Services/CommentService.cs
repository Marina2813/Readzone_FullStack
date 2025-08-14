using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class CommentService: ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CommentDto> AddCommentAsync(string postId, int userId, CreateCommentDto dto)
        {
            var post = await _unitOfWork.Posts.GetPostByIdAsync(postId)
                ?? throw new Exception("Post not found");

            var user = await _unitOfWork.Auth.GetByIdAsync(userId)
                ?? throw new Exception("User not found");

            var comment = _mapper.Map<Comment>(dto);
            comment.Name = user.Username;
            comment.UserId = userId;
            comment.PostId = postId;

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            var commentDto = _mapper.Map<CommentDto>(comment);
            commentDto.IsOwner = true; ;

            return commentDto;
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _unitOfWork.Comments.GetCommentByIdAsync(commentId);
            if (comment == null) return false;

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("You are not allowed to delete this comment.");

            return await _unitOfWork.Comments.DeleteCommentAsync(comment);


        }

        public async Task<IEnumerable<CommentDto>> GetCommentsForPostAsync(string postId, int? currentUserId)
        {
            var comments = await _unitOfWork.Comments.GetCommentsForPostAsync(postId);

            var commentDtos = comments
                .Select(c =>
                {
                    var dto = _mapper.Map<CommentDto>(c);
                    dto.IsOwner = currentUserId != null && c.UserId == currentUserId;
                    return dto;
                });

            return commentDtos;
        }

        public async Task<int> GetCommentCount(string postId)
        {
            return await _unitOfWork.Comments.GetCommentCountAsync(postId);
        }
    }
}

