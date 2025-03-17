using FileService.Domain;
using GlobalConfigurations.注册所有项目中的服务;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure
{
    public class ComponentInitializer : IComponetInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<ICloundClient, LocalCloundClient>();
            services.AddScoped<FileDomainService>();
        }
    }
}
