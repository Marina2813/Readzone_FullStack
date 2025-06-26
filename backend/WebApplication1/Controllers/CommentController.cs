using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        //Add a comment
        [Authorize]
        [HttpPost("{postId}")]
        public async Task<IActionResult> AddComment(string postId, [FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return NotFound("Post not found");

            var userIdClaim = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Unauthorized("User not found");

            var comment = new Comment
            {
                Name = user.Username,
                Content = dto.Content,
                Timestamp = DateTime.UtcNow,
                PostId = postId,
                UserId = userId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                comment.CommentId,
                comment.Name,
                comment.Content,
                comment.Timestamp
            });
        }


        //Delete a comment by ID
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return NotFound();

            var userIdClaim = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim);

            if (comment.UserId != userId) return Forbid();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Fetch comments for a post
        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsForPost(string postId)
        {
            var currentUserId = User?.FindFirstValue("UserId");

            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.Timestamp)
                .Select(c => new {
                    c.CommentId,
                    c.Name,
                    c.Content,
                    Timestamp = c.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    IsOwner = currentUserId != null && c.UserId.ToString() == currentUserId
                })
                .ToListAsync();

            return Ok(comments);
        }

        [HttpGet("count/{postId}")]
        public IActionResult GetCommentCount(string postId)
        {
            int count = _context.Comments.Count(c => c.PostId == postId);
            return Ok(count);
        }

    }
}
