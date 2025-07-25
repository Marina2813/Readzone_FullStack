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
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

            //Add a comment
        [Authorize]
        [HttpPost("{postId}")]
        public async Task<IActionResult> AddComment(string postId, [FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var comment = await _commentService.AddCommentAsync(postId, userId, dto.Content);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        //Delete a comment by ID
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var success = await _commentService.DeleteCommentAsync(commentId, userId);
                return success ? NoContent() : NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
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
