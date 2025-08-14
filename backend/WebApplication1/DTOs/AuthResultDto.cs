namespace WebApplication1.DTOs
{
    public record AuthResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AuthResponseDto? Tokens { get; set; } 
    }
}
