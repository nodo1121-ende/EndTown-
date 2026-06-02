using Microsoft.EntityFrameworkCore;
using EndTown.Models.Entities;

namespace EndTown.Data
{
    public class EndTownDbContext : DbContext
    {
        public EndTownDbContext(DbContextOptions<EndTownDbContext> options)
            : base(options)
        {
        }


        public DbSet<Platform> Platforms { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Platform>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(500);

                entity.HasIndex(e => e.Name)
                      .IsUnique(); // თუ გინდა რომ პლატფორმის სახელი უნიკალური იყოს
            });

            base.OnModelCreating(builder);
        }
    }
}