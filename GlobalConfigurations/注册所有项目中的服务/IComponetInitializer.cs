using Microsoft.Extensions.DependencyInjection;

namespace GlobalConfigurations.注册所有项目中的服务
{
    /// <summary>
    /// 注册模块中需要的服务
    /// </summary>
    public interface IComponetInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"> IServiceCollection依赖注入的容器</param>
        public void Initialize(IServiceCollection services);
    }
}