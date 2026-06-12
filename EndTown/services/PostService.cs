using EndTown.Data;
using EndTown.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EndTown.Services
{
    public class PostService : IPostService
    {
        private readonly EndTownDbContext _context;

        public PostService(EndTownDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostResponseDto>> GetAllAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount
                })
                .ToListAsync();
        }

        public async Task<PostResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PostResponseDto> CreateAsync(int userId, CreatePostRequest request)
        {
            var user = await _context.Users.FindAsync(userId);

            var post = new Post
            {
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return new PostResponseDto
            {
                Id = post.Id,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId,
                Username = user?.Username ?? "",
                LikesCount = 0,
                CommentsCount = 0
            };
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.UserId != userId)
                return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LikePostAsync(int postId, int userId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return false;

            var exists = await _context.Likes
                .AnyAsync(l => l.PostId == postId && l.UserId == userId);
            if (exists) return false;

            _context.Likes.Add(new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });

            post.LikesCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlikePostAsync(int postId, int userId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (like == null) return false;

            _context.Likes.Remove(like);

            var post = await _context.Posts.FindAsync(postId);
            if (post != null) post.LikesCount--;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCommentAsync(int postId, int userId, string content)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return false;

            _context.Comments.Add(new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            });

            post.CommentsCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetCommentsAsync(int postId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentResponseDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserId = c.UserId,
                    Username = c.User.Username,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }
    }
}