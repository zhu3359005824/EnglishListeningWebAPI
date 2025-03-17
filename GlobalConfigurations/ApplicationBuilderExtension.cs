using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConfigurations
{/// <summary>
/// 应用配置拓展
/// </summary>
    public static class ApplicationBuilderExtension
    {

        public static IApplicationBuilder 
            UseZhzDefault(this IApplicationBuilder app)
        {

            app.UseCors();

            app.UseForwardedHeaders();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;



        }
    }
}
