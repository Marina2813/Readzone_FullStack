using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikeController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("toggle")]
        public IActionResult ToggleLike([FromBody] Like likeRequest)
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail)) return Unauthorized("User email not found in token");

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return Unauthorized("User not found");

            string postId = likeRequest.PostId;

            var existingLike = _context.Likes.FirstOrDefault(l => l.UserId == user.Id && l.PostId == postId);
            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
            }
            else
            {
                var newLike = new Like
                {
                    UserId = user.Id,
                    PostId = postId,
                    LikedAt = DateTime.UtcNow
                };
                _context.Likes.Add(newLike);
            }

            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("count/{postId}")]
        public IActionResult GetLikeCount(string postId)
        {
            int count = _context.Likes.Count(l => l.PostId == postId);
            return Ok(count);
        }

        [Authorize]
        [HttpGet("userliked/{postId}")]
        public IActionResult UserLiked(string postId)
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail)) return Unauthorized("User email not found in token");

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return Unauthorized("User not found");

            var liked = _context.Likes.Any(l => l.UserId == user.Id && l.PostId == postId);
            return Ok(liked);
        }
    }
}
