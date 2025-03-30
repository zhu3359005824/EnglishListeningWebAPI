using GlobalConfigurations.注册所有项目中的服务;
using Microsoft.Extensions.DependencyInjection;

namespace MediaEncoder.Domain
{
    public class ComponentInitializer : IComponetInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<MediaEncoderFactory>();
        }
    }
}
