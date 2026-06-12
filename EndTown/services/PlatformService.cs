using EndTown.Data;
using EndTown.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EndTown.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly EndTownDbContext _context;

        public PlatformService(EndTownDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Platform>> GetAllAsync()
        {
            return await _context.Platforms
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Platform?> GetByIdAsync(int id)
        {
            return await _context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Platform> CreateAsync(Platform platform)
        {
            platform.CreatedAt = DateTime.UtcNow;
            platform.UpdatedAt = DateTime.UtcNow;

            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync();

            return platform;
        }

        public async Task<Platform?> UpdateAsync(int id, Platform platform)
        {
            var existing = await _context.Platforms.FindAsync(id);
            if (existing == null) return null;

            existing.Name = platform.Name;
            existing.Description = platform.Description;
            existing.LogoUrl = platform.LogoUrl;
            existing.BannerUrl = platform.BannerUrl;
            existing.RegistrationOpen = platform.RegistrationOpen;
            existing.PublicAccess = platform.PublicAccess;
            existing.MaxPostLength = platform.MaxPostLength;
            existing.MaxCommentLength = platform.MaxCommentLength;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null) return false;

            _context.Platforms.Remove(platform);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Platforms
                .AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }
    }
}