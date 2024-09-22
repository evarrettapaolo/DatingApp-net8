namespace API.Entities
{
  public class AppUser
  {
    public int Id { get; set; } // auto incremented
    public required string UserName { get; set; }
  }
}