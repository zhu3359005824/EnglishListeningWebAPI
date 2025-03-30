using System.Security.Claims;

namespace ZHZ.JWT
{
    public interface IJwtTokenService
    {
        string BuildToken(IEnumerable<Claim> claims, JWTSettings settings);
    }
}
