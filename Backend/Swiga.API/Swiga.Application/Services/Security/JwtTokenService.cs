using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swiga.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Swiga.Application.Services.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public (string token, DateTime expiresAtUtc) Create(UserModel user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = jwt["Key"]!;
            var issuer = jwt["Issuer"]!;
            var audience = jwt["Audience"]!;
            var minutes = int.Parse(jwt["ExpiresMinutes"] ?? "120");

            var expiresAtUtc = DateTime.UtcNow.AddMinutes(minutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())

            };


            if (user is AdminModel admin)
            {
                claims.Add(new Claim("rentalPointId", admin.RentalPointId.ToString()));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAtUtc,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expiresAtUtc);
        }
    }
}
