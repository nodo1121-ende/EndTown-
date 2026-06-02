// EndTown/Models/DTOs/PlatformResponseDto.cs
namespace EndTown.Models.DTOs
{
    public class PlatformResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string BannerUrl { get; set; } = string.Empty;

        // სტატისტიკა
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }

        // კონფიგურაცია
        public bool RegistrationOpen { get; set; }
        public bool PublicAccess { get; set; }
        public int MaxPostLength { get; set; }
        public int MaxCommentLength { get; set; }

        // ფიქსირებული ველები, არ შეცვლება
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}