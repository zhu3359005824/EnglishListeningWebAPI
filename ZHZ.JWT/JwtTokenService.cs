using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.JWT
{
    public class JwtTokenService : IJwtTokenService
    {
        public string BuildToken(IEnumerable<Claim> claims, JWTSettings settings)
        {
          TimeSpan expireTime= TimeSpan.FromSeconds(settings.ExpireSeconds);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(settings.Issuer, settings.Audience, claims,
                expires: DateTime.Now.Add(expireTime), signingCredentials: credentials);
            return  new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
