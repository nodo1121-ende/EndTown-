using EndTown.Models.Entities;

namespace EndTown.Services
{
    public class CreatePostRequest
    {
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

    }
    public class AddCommentRequest
    {
        public string Content { get; set; } = string.Empty;
    }
    public interface IPostService
    {
        Task<IEnumerable<PostResponseDto>> GetAllAsync();
        Task<PostResponseDto?> GetByIdAsync(int id);
        Task<PostResponseDto> CreateAsync(int userId, CreatePostRequest request);
        Task<bool> DeleteAsync(int id, int userId);
        Task<bool> LikePostAsync(int postId, int userId);
        Task<bool> UnlikePostAsync(int postId, int userId);
        Task<bool> AddCommentAsync(int postId, int userId, string content);
        Task<IEnumerable<CommentResponseDto>> GetCommentsAsync(int postId);
    }
}