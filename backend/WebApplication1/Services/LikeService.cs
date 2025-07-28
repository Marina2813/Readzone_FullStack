using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;

    public LikeService(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public async Task ToggleLikeAsync(string userEmail, string postId)
    {
        var user = await _likeRepository.GetUserByEmailAsync(userEmail);
        if (user == null) throw new Exception("User not found");

        var existingLike = await _likeRepository.GetLikeAsync(user.Id, postId);

        if (existingLike != null)
        {
            _likeRepository.Remove(existingLike);
            await _likeRepository.SaveChangesAsync();
        }
        else
        {
            var newLike = new Like
            {
                UserId = user.Id,
                PostId = postId,
                LikedAt = DateTime.UtcNow
            };
            await _likeRepository.AddAsync(newLike);
            await _likeRepository.SaveChangesAsync();
        }
    }

    public async Task<int> GetLikeCountAsync(string postId)
    {
        return await _likeRepository.GetLikeCountAsync(postId);
    }

    public async Task<bool> HasUserLikedPostAsync(string userEmail, string postId)
    {
        var user = await _likeRepository.GetUserByEmailAsync(userEmail);
        if (user == null) throw new Exception("User not found");

        return await _likeRepository.HasUserLikedPostAsync(user.Id, postId);
    }
}
