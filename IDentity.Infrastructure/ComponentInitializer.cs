
using GlobalConfigurations.注册所有项目中的服务;
using IDentity.Domain;
using Microsoft.Extensions.DependencyInjection;
using ZHZ.JWT;

namespace IDentity.Infrastructure
{
    public class ComponentInitializer : IComponetInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IdentityDomainService>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

        }
    }
}
