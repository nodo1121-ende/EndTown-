using EndTown.Models.Entities;

namespace EndTown.Services
{
    public interface IFriendshipService
    {
        Task<bool> SendRequestAsync(int senderId, int receiverId);
        Task<bool> AcceptRequestAsync(int friendshipId, int userId);
        Task<bool> RejectRequestAsync(int friendshipId, int userId);
        Task<IEnumerable<UserResponseDto>> GetFriendsAsync(int userId);
        Task<IEnumerable<FriendRequestDto>> GetPendingRequestsAsync(int userId);
    }

    public class FriendRequestDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}