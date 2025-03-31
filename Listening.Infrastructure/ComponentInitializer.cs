using GlobalConfigurations.注册所有项目中的服务;
using Listening.Domain;
using Listening.Domain.SentenceParser;
using Microsoft.Extensions.DependencyInjection;

namespace Listening.Infrastructure
{
    public class ComponentInitializer : IComponetInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IListeningRepository, ListeningRepository>();
            services.AddScoped<ListeningDomainService>();
            services.AddScoped<ISentenceParser, SrtParser>();
            services.AddScoped<ISentenceParser, LrcParser>();
            services.AddScoped<ParserFactory>();

        }
    }
}
