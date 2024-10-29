using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
  public class LogUserActivity : IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var resultContext = await next(); // * move to next middleware

      if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

      var userId = resultContext.HttpContext.User.GetUserId();

      var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
      var user = await repo.GetUserByIdAsync(userId);

      if (user == null) return;

      user.LastActive = DateTime.UtcNow; // * update last active
      await repo.SaveAllAsync();
    }
  }
}