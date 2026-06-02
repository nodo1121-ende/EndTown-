using System.ComponentModel.DataAnnotations;

namespace EndTown.Models.Entities
{
    public class Platform
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;
        public string BannerUrl { get; set; } = string.Empty;

        // სტატისტიკა
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }

        // კონფიგურაცია
        public bool RegistrationOpen { get; set; } = true;
        public bool PublicAccess { get; set; } = true;
        public int MaxPostLength { get; set;  } = 5000;
        public int MaxCommentLength { get; set; } = 1000;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // მეთოდები
        public void UpdateStatistics(int users, int posts, int comments, int likes)
        {
            TotalUsers = users;
            TotalPosts = posts;
            TotalComments = comments;
            TotalLikes = likes;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanUserRegister() => RegistrationOpen;

        public bool IsContentLengthValid(string content, ContentType contentType) => contentType switch
        {
            ContentType.Post => content.Length <= MaxPostLength,
            ContentType.Comment => content.Length <= MaxCommentLength,
            _ => true
        };
    }

    public enum ContentType
    {
        Post,
        Comment
    }
}