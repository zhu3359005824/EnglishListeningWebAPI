using FluentValidation;
using FluentValidation.AspNetCore;
using GlobalConfigurations.AllDbContextInitializer;
using GlobalConfigurations.注册所有项目中的服务;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using System.Threading.Tasks;
using ZHZ.EventBus;
using ZHZ.EventBus.RabbitMQ;
using ZHZ.Infrastructure.MediatR;
using ZHZ.JWT;
using ZHZ.Tools;
using ZHZ.UnitOkWork;

namespace GlobalConfigurations
{
    public static class WebApplicationBuilderExtension
    {
        public  static void  ConfigureExtensionService(this WebApplicationBuilder builder,InitializerOptions initializerOptions)
        {
            builder.Configuration.AddEnvironmentVariables();
            //依赖注入容器
            IServiceCollection service = builder.Services;

            IConfiguration configuration=
                builder.Configuration;

           IEnumerable<Assembly> assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            service.RunComponentInitializer(assemblies);


            service.AddAllDbContext(ctx =>
            {
                //连接字符串如果放到appsettings.json中，会有泄密的风险
                //如果放到UserSecrets中，每个项目都要配置，很麻烦
                //因此这里推荐放到环境变量中。
                string connStr = "Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
                ctx.UseSqlServer(connStr);
            }, assemblies);


            //-------Authentication  Authorization----------//
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

          
           string c =Environment.GetEnvironmentVariable("JWTSettings");
            JWTSettings jWTSettings = JsonConvert.DeserializeObject<JWTSettings>(c);
            builder.Services.Configure<JWTSettings>(options =>
            {
                options.Issuer = jWTSettings.Issuer;
                options.Audience = jWTSettings.Audience;
                options.Key = jWTSettings.Key;
                options.ExpireMinutes = jWTSettings.ExpireMinutes;
            });

            builder.Services.AddJWTAuthentication(jWTSettings);

            //启用Swagger中的[Authorize]

            builder.Services.Configure<SwaggerGenOptions>(c =>
            {
                c.AddAuthenticationHeader();
            });

            #region 数据验证
            //-------------------------//

            //----启用数据验证--------------------//
            //-----IValidationData的类都将进行数据验证---------------


            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            //  过滤掉系统/第三方程序集
            var filteredAssemblies = assemblies
                .Where(assembly =>
                    !assembly.FullName.StartsWith("System") &&
                    !assembly.FullName.StartsWith("Microsoft") &&
                    !assembly.FullName.StartsWith("FluentValidation"))
                .ToList();

            //  筛选包含 IValidationData 实现的程序集
            var assembliesWithValidationData = filteredAssemblies
                .Where(assembly =>
                {
                    try
                    {
                        return assembly.GetExportedTypes()
                            .Any(type =>
                                typeof(IValidationData).IsAssignableFrom(type) &&
                                !type.IsInterface &&
                                !type.IsAbstract
                            );
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        return false;
                    }
                })
                .ToList();

            //  筛选包含验证器的程序集
            var assembliesWithValidators = filteredAssemblies
                .Where(assembly =>
                {
                    try
                    {
                        return assembly.GetExportedTypes()
                            .Any(type =>
                                type.IsClass &&
                                !type.IsAbstract &&
                                type.BaseType != null &&
                                type.BaseType.IsGenericType &&
                                type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>) &&
                                typeof(IValidationData).IsAssignableFrom(type.BaseType.GetGenericArguments()[0])
                            );
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        return false;
                    }
                })
                .ToList();

            //  合并结果并去重
            var targetAssemblies = assembliesWithValidationData
                .Union(assembliesWithValidators)
                .Distinct()
                .ToList();

            // 注册这些程序集中的所有验证器
            foreach (var assembly in targetAssemblies)
            {
                builder.Services.AddValidatorsFromAssembly(assembly);
                builder.Services.AddValidatorsFromAssemblyContaining(assembly.GetType());
            }
            #endregion



            service.AddMediatR(assemblies);





            service.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<UnitOfWorkActionFilter>();
            });
            service.Configure<JsonOptions>(options =>
            {
                //设置时间格式。而非“2008-08-08T08:08:08”这样的格式
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
            });

            //添加跨域
            service.AddCors(options =>
            {
                //更好的在Program.cs中用绑定方式读取配置的方法：https://github.com/dotnet/aspnetcore/issues/21491
                //不过比较麻烦。
                string[] urls = {""};
                options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            }
            );


            
            
            service.Configure<IntergrationEventRabbitMQOption>(configuration.GetSection("RabbitMQ"));

            service.AddEventBus(initializerOptions.EventBusQueueName, assemblies);

            //Redis的配置
            string redisConnStr = configuration.GetValue<string>("Redis:ConnStr");
            IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
            service.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);
            service.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }
    }
}
