using EndTown.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


namespace EndTown.Data
{
    public class EndTownDbContext : DbContext
    {
        public EndTownDbContext(DbContextOptions<EndTownDbContext> options)
            : base(options) { }

        public DbSet<Platform> Platforms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<FriendShips> FriendShips { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageFollower> PageFollowers { get; set; }
        public DbSet<EndTown.Models.Entities.Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>().HasKey(p => p.Id);

            builder.Entity<Platform>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

       
            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            builder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(p => p.User)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Post)
                      .WithMany(p => p.Comments)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

    
            builder.Entity<Like>(entity =>
            {
                entity.HasOne(l => l.User)
                      .WithMany()
                      .HasForeignKey(l => l.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(l => l.Post)
                      .WithMany(p => p.Likes)
                      .HasForeignKey(l => l.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<FriendShips>(entity =>
            {
                entity.HasOne(f => f.Sender)
                      .WithMany()
                      .HasForeignKey(f => f.SenderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(f => f.Receiver)
                      .WithMany(u => u.Friendships)
                      .HasForeignKey(f => f.ReceiverId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<Page>(entity =>
            {
                entity.HasOne(p => p.Owner)
                      .WithMany()
                      .HasForeignKey(p => p.OwnerId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<PageFollower>(entity =>
            {
                entity.HasOne(f => f.Page)
                      .WithMany(p => p.Followers)
                      .HasForeignKey(f => f.PageId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.User)
                      .WithMany()
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<EndTown.Models.Entities.Group>(entity =>
            {
                entity.HasOne(g => g.Owner)
                      .WithMany()
                      .HasForeignKey(g => g.OwnerId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<GroupMember>(entity =>
            {
                entity.HasOne(m => m.Group)
                      .WithMany(g => g.Members)
                      .HasForeignKey(m => m.GroupId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.User)
                      .WithMany()
                      .HasForeignKey(m => m.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            base.OnModelCreating(builder);
        }
    }
}