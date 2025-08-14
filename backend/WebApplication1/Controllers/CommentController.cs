using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

            //Add a comment
        [HttpPost("{postId}")]
        public async Task<IActionResult> AddComment(string postId, [FromBody] CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var comment = await _commentService.AddCommentAsync(postId, userId, dto);
            _logger.LogInformation("Comment added successfully by user {UserId} to post {PostId}", userId, postId);

            return Ok(comment);

        }
        


        //Delete a comment by ID
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var success = await _commentService.DeleteCommentAsync(commentId, userId);
            if (!success)
            {
                _logger.LogWarning("Delete failed: Comment {CommentId} not found or user {UserId} not authorized", commentId, userId);
                return NotFound(new { message = "Comment not found or not authorized." });
            }

            _logger.LogInformation("Comment {CommentId} deleted by user {UserId}", commentId, userId);
            return NoContent(); 
        }

        //Fetch comments for a post
        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsForPost(string postId)
        {
            int? currentUserId = null;
            var idClaim = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (idClaim != null) currentUserId = int.Parse(idClaim);

            var comments = await _commentService.GetCommentsForPostAsync(postId, currentUserId);
            return Ok(comments);
        }

        [HttpGet("count/{postId}")]
        public async Task<IActionResult> GetCommentCount(string postId)
        {
            var count = await _commentService.GetCommentCount(postId); 
            return Ok(count);
        }

    }
}
