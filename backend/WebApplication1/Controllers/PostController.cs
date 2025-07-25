using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }


        //create post
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim);
            var createdPost = await _postService.CreatePostAsync(post, userId);
            return Ok(createdPost);
        }


        // get all posts

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return Ok(await _postService.GetAllPostsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(string id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }


        // update post

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] Post updatedPost)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (await _postService.UpdatePostAsync(id, updatedPost, userEmail!))
                return NoContent();

            return Forbid("Unauthorized or Post not found");
        }

        // delete post
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (await _postService.DeletePostAsync(id, userEmail!))
                return NoContent();

            return Forbid("Unauthorized or Post not found");
        }

        [HttpGet("myposts")]
        public async Task<IActionResult> GetUserPosts()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(await _postService.GetUserPostsAsync(userEmail!));
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query string cannot be empty.");

            return Ok(await _postService.SearchPostsAsync(query));
        }

    }
}
