using JWT;
using Microsoft.IdentityModel.Tokens;
using Nexora.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Nexora.cookie
{
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace Nexora.Cookie
    {
        public class JwtService
        {
            private readonly string _jwtSecret;
            private readonly string _issuer;
            private readonly string _audience;


            public JwtService(IConfiguration configuration)
            {
                _jwtSecret = configuration["Jwt:Secret"]
                    ?? throw new InvalidOperationException("JWT secret not configured");
                _issuer = configuration["Jwt:Issuer"]
                    ?? throw new InvalidOperationException("JWT issuer not configured");
                _audience = configuration["Jwt:Audience"]
                    ?? throw new InvalidOperationException("JWT audience not configured");
            }

            public string CreateJwtToken(int userId, string role, int mandantId)
            {
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("mandant_id", mandantId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: creds
                );

               

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
