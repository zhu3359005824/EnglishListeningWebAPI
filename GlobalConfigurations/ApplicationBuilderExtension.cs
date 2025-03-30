using Microsoft.AspNetCore.Builder;

namespace GlobalConfigurations
{/// <summary>
/// 应用配置拓展
/// </summary>
    public static class ApplicationBuilderExtension
    {

        public static IApplicationBuilder
            UseZhzDefault(this IApplicationBuilder app)
        {
            // app.UseEventBus();
            app.UseCors();

            app.UseForwardedHeaders();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;



        }
    }
}
