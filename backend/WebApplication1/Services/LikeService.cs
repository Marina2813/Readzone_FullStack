using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ToggleLikeAsync(string userEmail, string postId)
    {
        var user = await _unitOfWork.Auth.GetUserByEmailAsync(userEmail);
        if (user == null) throw new Exception("User not found");

        var existingLike = await _unitOfWork.Likes.GetLikeAsync(user.Id, postId);

        if (existingLike != null)
        {
            _unitOfWork.Likes.Remove(existingLike);
            
        }
        else
        {
            var newLike = new Like
            {
                UserId = user.Id,
                PostId = postId,
                LikedAt = DateTime.UtcNow
            };
            await _unitOfWork.Likes.AddAsync(newLike);
            
        }
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetLikeCountAsync(string postId)
    {
        return await _unitOfWork.Likes.GetLikeCountAsync(postId);
    }

    public async Task<bool> HasUserLikedPostAsync(string userEmail, string postId)
    {
        var user = await _unitOfWork.Auth.GetUserByEmailAsync(userEmail);
        if (user == null) throw new Exception("User not found");

        return await _unitOfWork.Likes.HasUserLikedPostAsync(user.Id, postId);
    }
}
