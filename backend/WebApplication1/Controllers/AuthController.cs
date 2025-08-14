//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.configurations;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{

    [ApiVersion("1.0")] // defines the version
    [Route("api/v{version:apiVersion}/[controller]")] //  adds version in route
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var result = await _authService.RegisterAsync(userDto);

            if (result == "User already exists")
            {
                _logger.LogWarning("Registration failed: user already exists for email {Email}", userDto.Email);
                return BadRequest(result);
            }

            _logger.LogInformation("User registered successfully: {Email}", userDto.Email);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                _logger.LogWarning("Login failed for email {Email}: {Reason}", loginDto.Email, result.Message);
                return Unauthorized(result.Message);
            }

            _logger.LogInformation("Login successful for email {Email}", loginDto.Email);
            return Ok(result.Tokens);
            
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var username = await _authService.GetUsernameByIdAsync(id);
            if (username == null)
                return NotFound(new { message = "User not found" });

            return Ok(username);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            _logger.LogInformation("Password reset attempt for email {Email}", dto.Email);
            var result = await _authService.ResetPasswordAsync(dto);

            if (result == "User not found")
            {
                _logger.LogWarning("Password reset failed: user not found for email {Email}", dto.Email);
                return NotFound(result);

            }

            _logger.LogInformation("Password reset successful for email {Email}", dto.Email);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.Success)
                return Unauthorized(new { result.Message });

            return Ok(result.Tokens);
        }


    }
}
