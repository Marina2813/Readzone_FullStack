public interface ILikeService
{
    Task ToggleLikeAsync(string userEmail, string postId);
    Task<int> GetLikeCountAsync(string postId);
    Task<bool> HasUserLikedPostAsync(string userEmail, string postId);
}

