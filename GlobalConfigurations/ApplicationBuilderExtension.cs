using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.EventBus;

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
