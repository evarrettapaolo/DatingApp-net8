namespace API.Extensions
{
  public static class DateTimeExtensions
  {
    public static int CalculateAge(this DateOnly dob)
    {
      var today = DateOnly.FromDateTime(DateTime.Now);

      var age = today.Year - dob.Year;

      // * birthday has yet to arrive for current year
      if (dob > today.AddYears(-age)) age--;

      return age;
    }
  }
}