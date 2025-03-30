using GlobalConfigurations.注册所有项目中的服务;
using MediaEncoder.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace MediaEncoder.Domain
{
    public class ComponentInitializer : IComponetInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IMediaEncoder, ToM4AEncoder>();
            services.AddScoped<IMediaEncoderRepository, MediaEncoderRepository>();
        }
    }
}
