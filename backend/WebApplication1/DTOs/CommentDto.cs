namespace WebApplication1.DTOs
{
    public record CommentDto
    {
        public int CommentId { get; set; }
        public required string Name { get; set; }
        public required string Content { get; set; }
        public required string Timestamp { get; set; }
        public bool IsOwner { get; set; }
    }
}
