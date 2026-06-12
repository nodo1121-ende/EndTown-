using System.ComponentModel.DataAnnotations;

namespace EndTown.Models.Entities
{
    public class Page
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;

        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        public int FollowersCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<PageFollower> Followers { get; set; } = new();
    }

    public class PageFollower
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}