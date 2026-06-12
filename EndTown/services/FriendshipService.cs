using EndTown.Data;
using EndTown.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EndTown.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly EndTownDbContext _context;

        public FriendshipService(EndTownDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendRequestAsync(int senderId, int receiverId)
        {
            if (senderId == receiverId) return false;

            var exists = await _context.FriendShips.AnyAsync(f =>
                (f.SenderId == senderId && f.ReceiverId == receiverId) ||
                (f.SenderId == receiverId && f.ReceiverId == senderId));

            if (exists) return false;

            _context.FriendShips.Add(new FriendShips
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = FriendshipStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AcceptRequestAsync(int friendshipId, int userId)
        {
            var friendship = await _context.FriendShips
                .FirstOrDefaultAsync(f => f.Id == friendshipId && f.ReceiverId == userId);

            if (friendship == null) return false;

            friendship.Status = FriendshipStatus.Accepted;
            friendship.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectRequestAsync(int friendshipId, int userId)
        {
            var friendship = await _context.FriendShips
                .FirstOrDefaultAsync(f => f.Id == friendshipId && f.ReceiverId == userId);

            if (friendship == null) return false;

            friendship.Status = FriendshipStatus.Rejected;
            friendship.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserResponseDto>> GetFriendsAsync(int userId)
        {
            var friendships = await _context.FriendShips
                .Include(f => f.Sender)
                .Include(f => f.Receiver)
                .Where(f => (f.SenderId == userId || f.ReceiverId == userId)
                         && f.Status == FriendshipStatus.Accepted)
                .AsNoTracking()
                .ToListAsync();

            return friendships.Select(f =>
            {
                var friend = f.SenderId == userId ? f.Receiver : f.Sender;
                return new UserResponseDto
                {
                    Id = friend.Id,
                    Username = friend.Username,
                    Email = friend.Email,
                    Bio = friend.Bio,
                    AvatarUrl = friend.AvatarUrl,
                    CreatedAt = friend.CreatedAt
                };
            });
        }

        public async Task<IEnumerable<FriendRequestDto>> GetPendingRequestsAsync(int userId)
        {
            return await _context.FriendShips
                .Include(f => f.Sender)
                .Where(f => f.ReceiverId == userId && f.Status == FriendshipStatus.Pending)
                .AsNoTracking()
                .Select(f => new FriendRequestDto
                {
                    Id = f.Id,
                    SenderId = f.SenderId,
                    SenderUsername = f.Sender.Username,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();
        }
    }
}