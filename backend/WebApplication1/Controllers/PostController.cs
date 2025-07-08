using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        //create post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim);

            var userEmail = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return Unauthorized("User not found");

            var count = await _context.Posts.CountAsync();
            post.PostId = $"P-{Guid.NewGuid()}";
            post.UserId = userId;
            post.CreatedDate = DateTime.UtcNow;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }

        // get all posts

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(string id)
        {
            Console.WriteLine($"[DEBUG] Angular is asking for PostId = {id}");
            var post = await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                Console.WriteLine("[DEBUG] Post not found.");
                return NotFound();
            }

            Console.WriteLine($"[DEBUG] Found Post with title: {post.Title}");
            return post;
        }
        

        // update post

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] Post updatedPost)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (post == null) return NotFound("Post not found");

            var userEmail = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return Unauthorized("User not found");

            if (post.UserId != user.Id)
            {
                return Forbid("You are not allowed to edit this post");
            }
            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // delete post
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (post == null) return NotFound("Post not found");

            var userEmail = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return Unauthorized("User not found");

            
            if (post.UserId != user.Id)
            {
                return Forbid("You are not authorized to delete this post");
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("myposts")]
        public async Task<IActionResult> GetUserPosts()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("Email not found in token");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
                return Unauthorized("User not found");

            var posts = await _context.Posts
                .Where(p => p.UserId == user.Id)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query string cannot be empty.");

            var lowerQuery = query.ToLower();

            var results = await _context.Posts
                .Where(p => p.Title.ToLower().Contains(lowerQuery) || p.Content.ToLower().Contains(lowerQuery))
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return Ok(results);
        }

    }
}
