namespace API.Helpers
{
  public class UserParams : PaginationParams
  {
    public string? Gender { get; set; }
    public string? CurrentUsername { get; set; }  // * user to exclude user from the list
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
    public string OrderBy { get; set; } = "lastActive";
  }
}