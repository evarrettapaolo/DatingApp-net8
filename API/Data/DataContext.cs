using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  // * primary constructor
  public class DataContext(DbContextOptions options) : DbContext(options)
  {
    public DbSet<AppUser> Users { get; set; }
  }
}