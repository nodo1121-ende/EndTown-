using EndTown.Data;
using EndTown.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EndTown.Services
{
    public class GroupService : IGroupService
    {
        private readonly EndTownDbContext _context;

        public GroupService(EndTownDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupResponseDto>> GetAllAsync()
        {
            return await _context.Groups
                .Include(g => g.Owner)
                .AsNoTracking()
                .Select(g => new GroupResponseDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Privacy = g.Privacy.ToString(),
                    OwnerId = g.OwnerId,
                    OwnerUsername = g.Owner.Username,
                    MembersCount = g.MembersCount,
                    CreatedAt = g.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<GroupResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Owner)
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new GroupResponseDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Privacy = g.Privacy.ToString(),
                    OwnerId = g.OwnerId,
                    OwnerUsername = g.Owner.Username,
                    MembersCount = g.MembersCount,
                    CreatedAt = g.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<GroupResponseDto> CreateAsync(int userId, CreateGroupRequest request)
        {
            var user = await _context.Users.FindAsync(userId);

            var group = new Group
            {
                Name = request.Name,
                Description = request.Description,
                Privacy = request.Privacy == "Private" ? GroupPrivacy.Private : GroupPrivacy.Public,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            _context.GroupMembers.Add(new GroupMember
            {
                GroupId = group.Id,
                UserId = userId,
                Role = GroupMemberRole.Admin,
                JoinedAt = DateTime.UtcNow
            });

            group.MembersCount++;
            await _context.SaveChangesAsync();

            return new GroupResponseDto
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                Privacy = group.Privacy.ToString(),
                OwnerId = group.OwnerId,
                OwnerUsername = user?.Username ?? "",
                MembersCount = group.MembersCount,
                CreatedAt = group.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null || group.OwnerId != userId) return false;
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> JoinAsync(int groupId, int userId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null) return false;

            var exists = await _context.GroupMembers
                .AnyAsync(m => m.GroupId == groupId && m.UserId == userId);
            if (exists) return false;

            _context.GroupMembers.Add(new GroupMember
            {
                GroupId = groupId,
                UserId = userId,
                Role = GroupMemberRole.Member,
                JoinedAt = DateTime.UtcNow
            });

            group.MembersCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveAsync(int groupId, int userId)
        {
            var member = await _context.GroupMembers
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == userId);
            if (member == null) return false;

            _context.GroupMembers.Remove(member);

            var group = await _context.Groups.FindAsync(groupId);
            if (group != null) group.MembersCount--;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}