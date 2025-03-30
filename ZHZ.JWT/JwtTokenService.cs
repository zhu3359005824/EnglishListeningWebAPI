using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ZHZ.JWT
{
    public class JwtTokenService : IJwtTokenService
    {
        public string BuildToken(IEnumerable<Claim> claims, JWTSettings settings)
        {
            TimeSpan expireTime = TimeSpan.FromMinutes(settings.ExpireMinutes);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(settings.Issuer, settings.Audience, claims,
                expires: DateTime.Now.Add(expireTime), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
