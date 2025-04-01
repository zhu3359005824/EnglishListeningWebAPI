using FluentValidation;
using FluentValidation.AspNetCore;
using GlobalConfigurations.AllDbContextInitializer;
using GlobalConfigurations.注册所有项目中的服务;
using GlobalConfigurations.配置类;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        public static void ConfigureExtensionService(this WebApplicationBuilder builder, InitializerOptions initializerOptions)
        {
           // builder.Configuration.AddEnvironmentVariables();
            //依赖注入容器
            IServiceCollection service = builder.Services;
            IConfiguration configuration = builder.Configuration;



            string projectRootPath = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;
            string filePath = Path.Combine(projectRootPath, "GlobalConfigurations/config.json");



            builder.Configuration.AddJsonFile(filePath);


            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));
            builder.Services.Configure<IntergrationEventRabbitMQOption>(builder.Configuration.GetSection("IntergrationEventRabbitMQOption"));

            builder.Services.Configure<MyRedisOption>(builder.Configuration.GetSection("MyRedisOption"));







           






            // builder.Configuration.AddJsonFile();

            IEnumerable<Assembly> assemblies = ReflectionHelper.GetAllReferencedAssemblies();

            // 假设另一个程序的程序集文件路径


            // string mediaEncoder = Path.Combine(projectRootPath, "MediaEncoder.WebAPI/bin/Debug/net8.0/MediaEncoder.WebAPI.dll");


            // 加载另一个程序集
            // Assembly anotherAssembly = Assembly.LoadFrom(mediaEncoder);

            // 获取另一个程序集的入口点
            //MethodInfo entryPoint = anotherAssembly.EntryPoint;
            //Assembly anotherAssembly=null;
            //Assembly? rootAssembly = Assembly.GetEntryAssembly();
            ////string currentPath= Path.Combine(projectRootPath, "MediaEncoder.WebAPI/bin/Debug/net8.0/MediaEncoder.WebAPI.dll");
            //if (rootAssembly.FullName== "Listening.Admin.WebAPI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
            //{
            //    // 假设另一个程序的程序集文件路径


            //    string mediaEncoder = Path.Combine(projectRootPath, "MediaEncoder.WebAPI/bin/Debug/net8.0/MediaEncoder.WebAPI.dll");


            //    // 加载另一个程序集
            //  anotherAssembly = Assembly.LoadFrom(mediaEncoder);
            //}
            




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




            builder.Services.AddJWTAuthentication(configuration.GetSection("JWTSettings").Get<JWTSettings>());

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
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                // 2. 处理日期时间格式
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));

                // 3. 忽略循环引用
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                // 4. 允许注释（开发环境推荐）
                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;

                // 5. 处理枚举值为字符串
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            });

            //添加跨域
            service.AddCors(options =>
            {
                //更好的在Program.cs中用绑定方式读取配置的方法：https://github.com/dotnet/aspnetcore/issues/21491
                //不过比较麻烦。
                string[] urls = { "http://localhost:5173/" };
                options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            }
            );






            service.AddEventBus(initializerOptions.EventBusQueueName, assemblies);

            //Redis的配置
            string redisConnStr = configuration.GetSection("MyRedisOption").Get<MyRedisOption>().ConnectionString;
            IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
            service.AddSingleton(typeof(IConnectionMultiplexer), redisConnMultiplexer);
            service.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }
    }
}
