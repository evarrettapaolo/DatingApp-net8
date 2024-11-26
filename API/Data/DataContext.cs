using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  // * primary constructor
  public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int,
  IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
  IdentityUserToken<int>>(options)
  {
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }

    // * customize the table for UserLike feature
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // * identity, many to many relationship
      modelBuilder.Entity<AppUser>()
        .HasMany(ur => ur.UserRoles)
        .WithOne(u => u.User)
        .HasForeignKey(ur => ur.UserId)
        .IsRequired();

      modelBuilder.Entity<AppRole>()
        .HasMany(ur => ur.UserRoles)
        .WithOne(u => u.Role)
        .HasForeignKey(ur => ur.RoleId)
        .IsRequired();

      // * like feature, many to many relationship
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

      // * message feature, many to many relationship
      modelBuilder.Entity<Message>()
        .HasOne(x => x.Recipient)
        .WithMany(l => l.MessagesReceived)
        .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Message>()
        .HasOne(x => x.Sender)
        .WithMany(l => l.MessagesSent)
        .OnDelete(DeleteBehavior.Restrict);

    }
  }
}