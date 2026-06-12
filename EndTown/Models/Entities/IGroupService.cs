namespace EndTown.Services
{
    public class CreateGroupRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Privacy { get; set; } = "Public";
    }

    public class GroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Privacy { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public string OwnerUsername { get; set; } = string.Empty;
        public int MembersCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public interface IGroupService
    {
        Task<IEnumerable<GroupResponseDto>> GetAllAsync();
        Task<GroupResponseDto?> GetByIdAsync(int id);
        Task<GroupResponseDto> CreateAsync(int userId, CreateGroupRequest request);
        Task<bool> DeleteAsync(int id, int userId);
        Task<bool> JoinAsync(int groupId, int userId);
        Task<bool> LeaveAsync(int groupId, int userId);
    }
}