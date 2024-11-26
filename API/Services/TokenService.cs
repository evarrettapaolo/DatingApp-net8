using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
  public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ITokenService
  {
    public async Task<string> CreateToken(AppUser user)
    {
      // * token key
      var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");
      if (tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

      if(user.UserName == null) throw new Exception("No username for user");

      // * claims
      var claims = new List<Claim>
      {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.UserName),
      };

      // * roles
      var roles = await userManager.GetRolesAsync(user);
      claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

      // * security algorithm
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      // * execute helper class and methods
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = creds
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);

    }
  }
}