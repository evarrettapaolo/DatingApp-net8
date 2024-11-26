using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AdminController(UserManager<AppUser> userManager) : BaseApiController
  {
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUserWithRoles()
    {
      // * custom object
      var user = await userManager.Users
        .OrderBy(x => x.UserName)
        .Select(x => new
        {
          x.Id,
          Username = x.UserName,
          Roles = x.UserRoles.Select(r => r.Role.Name).ToArray()
        })
        .ToListAsync();

      return Ok(user);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, string roles)
    {
      if (string.IsNullOrEmpty(roles)) BadRequest("You must select at least one role");

      // * data gathering
      var selectedRoles = roles.Split(",").ToArray();

      var user = await userManager.FindByNameAsync(username);

      if (user == null) return BadRequest("User not found");

      var userRoles = await userManager.GetRolesAsync(user);

      // * changing role state
      var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

      if (!result.Succeeded) return BadRequest("Failed to add to roles");

      result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

      if (!result.Succeeded) return BadRequest("Failed to remove from roles");

      return Ok(await userManager.GetRolesAsync(user));
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public ActionResult GetPhotosForModeration()
    {
      return Ok("Admins or moderators can see this");
    }
  }
}