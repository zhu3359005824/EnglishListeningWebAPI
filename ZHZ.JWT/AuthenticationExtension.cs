using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ZHZ.JWT
{
    public static class AuthenticationExtension
    {
        public static AuthenticationBuilder
            AddJWTAuthentication(this IServiceCollection services, JWTSettings jWTSettings)
        {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new()
                {

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jWTSettings.Issuer,
                    ValidAudience = jWTSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTSettings.Key)),

                };
            });
        }

    }
}
