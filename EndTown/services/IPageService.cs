using EndTown.Models.Entities;

namespace EndTown.Services
{
    public class CreatePageRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }

    public class PageResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public string OwnerUsername { get; set; } = string.Empty;
        public int FollowersCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public interface IPageService
    {
        Task<IEnumerable<PageResponseDto>> GetAllAsync();
        Task<PageResponseDto?> GetByIdAsync(int id);
        Task<PageResponseDto> CreateAsync(int userId, CreatePageRequest request);
        Task<bool> DeleteAsync(int id, int userId);
        Task<bool> FollowAsync(int pageId, int userId);
        Task<bool> UnfollowAsync(int pageId, int userId);
    }
}