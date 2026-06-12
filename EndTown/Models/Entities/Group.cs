using System.ComponentModel.DataAnnotations;

namespace EndTown.Models.Entities
{
    public enum GroupPrivacy { Public, Private }

    public class Group
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public GroupPrivacy Privacy { get; set; } = GroupPrivacy.Public;
        public string CoverUrl { get; set; } = string.Empty;

        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        public int MembersCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<GroupMember> Members { get; set; } = new();
    }

    public enum GroupMemberRole { Member, Admin }

    public class GroupMember
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public GroupMemberRole Role { get; set; } = GroupMemberRole.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}