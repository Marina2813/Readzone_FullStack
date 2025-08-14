namespace WebApplication1.DTOs
{
    public record PostDto
    {
        public required string PostId { get; init; }
        public required string Title { get; init; }
        public required string Content { get; init; }
        public required string CreatedDate { get; init; }
        public int UserId { get; init; }
        public string Username { get; init; } = "Unknown";
        public string Category { get; init; } = "Anonymous";
    }
}
