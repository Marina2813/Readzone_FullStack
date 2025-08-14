using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories;


namespace WebApplication1.Services
{
    public class AuthService: IAuthService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService(IConfiguration config, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _config = config;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> RegisterAsync(UserDto userDto)
        {
            if (await _unitOfWork.Auth.UserExistsAsync(userDto.Email))
                return "User already exists";

            var user = _mapper.Map<User>(userDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);


            await _unitOfWork.Auth.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return "User registered successfully";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _unitOfWork.Auth.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return "User not found";

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _unitOfWork.SaveChangesAsync();

            return "Password updated successfully";
        }


        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Auth.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Invalid credentials"
                };
            }

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResultDto
            {
                Success = true,
                Tokens = new AuthResponseDto(accessToken, refreshToken)
            };

        }

        public async Task<string?> GetUsernameByIdAsync(int id)
        {
            var user = await _unitOfWork.Auth.GetByIdAsync(id); 
            return user?.Username;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Auth.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = "Invalid or expired refresh token"
                };
            }

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResultDto
            {
                Success = true,
                Tokens = new AuthResponseDto(newAccessToken, newRefreshToken)
            };
        }


        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

    }
}

