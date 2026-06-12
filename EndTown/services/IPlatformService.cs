using EndTown.Models.Entities;

namespace EndTown.Services
{
    public interface IPlatformService
    {
        Task<IEnumerable<Platform>> GetAllAsync();
        Task<Platform?> GetByIdAsync(int id);
        Task<Platform> CreateAsync(Platform platform);
        Task<Platform?> UpdateAsync(int id, Platform platform);
        Task<bool> DeleteAsync(int id);
        Task<bool> NameExistsAsync(string name);
    }
}