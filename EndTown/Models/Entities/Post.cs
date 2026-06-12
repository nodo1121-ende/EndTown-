using System.ComponentModel.DataAnnotations;



namespace EndTown.Models.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    
        public int UserId { get; set; }
        public User User { get; set; } = null!;

    
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }

     
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}