using EndTown.Data;
using EndTown.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EndTown.Services
{
    public class PageService : IPageService
    {
        private readonly EndTownDbContext _context;

        public PageService(EndTownDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PageResponseDto>> GetAllAsync()
        {
            return await _context.Pages
                .Include(p => p.Owner)
                .AsNoTracking()
                .Select(p => new PageResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    AvatarUrl = p.AvatarUrl,
                    OwnerId = p.OwnerId,
                    OwnerUsername = p.Owner.Username,
                    FollowersCount = p.FollowersCount,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<PageResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Pages
                .Include(p => p.Owner)
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PageResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    AvatarUrl = p.AvatarUrl,
                    OwnerId = p.OwnerId,
                    OwnerUsername = p.Owner.Username,
                    FollowersCount = p.FollowersCount,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PageResponseDto> CreateAsync(int userId, CreatePageRequest request)
        {
            var user = await _context.Users.FindAsync(userId);

            var page = new Page
            {
                Name = request.Name,
                Description = request.Description,
                AvatarUrl = request.AvatarUrl,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Pages.Add(page);
            await _context.SaveChangesAsync();

            return new PageResponseDto
            {
                Id = page.Id,
                Name = page.Name,
                Description = page.Description,
                AvatarUrl = page.AvatarUrl,
                OwnerId = page.OwnerId,
                OwnerUsername = user?.Username ?? "",
                CreatedAt = page.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null || page.OwnerId != userId) return false;
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> FollowAsync(int pageId, int userId)
        {
            var page = await _context.Pages.FindAsync(pageId);
            if (page == null) return false;

            var exists = await _context.PageFollowers
                .AnyAsync(f => f.PageId == pageId && f.UserId == userId);
            if (exists) return false;

            _context.PageFollowers.Add(new PageFollower
            {
                PageId = pageId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });

            page.FollowersCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnfollowAsync(int pageId, int userId)
        {
            var follower = await _context.PageFollowers
                .FirstOrDefaultAsync(f => f.PageId == pageId && f.UserId == userId);
            if (follower == null) return false;

            _context.PageFollowers.Remove(follower);

            var page = await _context.Pages.FindAsync(pageId);
            if (page != null) page.FollowersCount--;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}