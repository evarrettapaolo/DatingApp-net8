using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  // * primary constructor
  public class DataContext(DbContextOptions options) : DbContext(options)
  {
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }

    // * customize the table for UserLike feature
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // * like feature many to many relationship
      modelBuilder.Entity<UserLike>()
        .HasKey(k => new { k.SourceUserId, k.TargetUserId });

      // * Liked users
      modelBuilder.Entity<UserLike>()
        .HasOne(s => s.SourceUser)
        .WithMany(l => l.LikedUsers)
        .HasForeignKey(s => s.SourceUserId)
        .OnDelete(DeleteBehavior.Cascade);

      // * Liked by users
      modelBuilder.Entity<UserLike>()
        .HasOne(s => s.TargetUser)
        .WithMany(l => l.LikedByUsers)
        .HasForeignKey(s => s.TargetUserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}