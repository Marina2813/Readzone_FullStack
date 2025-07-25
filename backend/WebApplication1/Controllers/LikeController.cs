using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [Authorize]
        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleLike([FromBody] Like likeRequest)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail)) return Unauthorized("User email not found in token");

            try
            {
                await _likeService.ToggleLikeAsync(userEmail, likeRequest.PostId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("count/{postId}")]
        public async Task<IActionResult> GetLikeCount(string postId)
        {
            int count = await _likeService.GetLikeCountAsync(postId);
            return Ok(count);
        }

        [Authorize]
        [HttpGet("userliked/{postId}")]
        public async Task<IActionResult> UserLiked(string postId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail)) return Unauthorized("User email not found in token");

            try
            {
                bool liked = await _likeService.HasUserLikedPostAsync(userEmail, postId);
                return Ok(liked);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
