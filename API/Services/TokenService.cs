using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
  public class TokenService(IConfiguration config) : ITokenService
  {
    public string CreateToken(AppUser user)
    {
      // * token key
      var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");
      if (tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

      // * claims
      var claims = new List<Claim>
      {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.UserName),
      };

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