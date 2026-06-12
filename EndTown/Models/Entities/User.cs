using System.ComponentModel.DataAnnotations;

namespace EndTown.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(200)]
        public string Bio { get; set; } = string.Empty;

        public string AvatarUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public List<Post> Posts { get; set; } = new List<Post>();
        public List<FriendShips> Friendships { get; set; } = new List<FriendShips>();
    }
}