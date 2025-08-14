using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }


        //create post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim);
            var createdPost = await _postService.CreatePostAsync(dto, userId);
            return Ok(createdPost);
        }


        // get all posts

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedResult<PostDto>>> GetPosts([FromQuery] PaginationParamsDto pagination)
        {
            var postsPagedResult = await _postService.GetAllPostsAsync(pagination);
            return Ok(postsPagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(string id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }


        // update post

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] CreatePostDto updatedDto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("User email not found in token");

            var result = await _postService.UpdatePostAsync(id, updatedDto, userEmail);
            if (result)
                return Ok("Post updated successfully");

            return Forbid("Unauthorized or Post not found");
        }

        // delete post
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _postService.DeletePostAsync(id, userEmail!);

            if (result)
                return Ok("Post deleted successfully");

            return Forbid("Unauthorized or post not found");
        }

        //get a users post
        [HttpGet("myposts")]
        public async Task<IActionResult> GetUserPosts()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("Email not found");

            var posts = await _postService.GetUserPostsAsync(userEmail);
            return Ok(posts);
        }

        //search
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query cannot be empty");

            var results = await _postService.SearchPostsAsync(query);
            return Ok(results);
        }

    }
}
