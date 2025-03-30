using GlobalConfigurations;
using IDentity.Domain.Entity;
using IDentity.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace IDentity.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureExtensionService(new InitializerOptions()
            {
                EventBusQueueName = "1",
                LogFilePath = "E:/Identity.log"
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "IdentityService.WebAPI", Version = "v1" });
                //c.AddAuthenticationHeader();
            });




            //builder.Services.AddFluentValidationAutoValidation();
            //builder.Services.AddFluentValidationClientsideAdapters();
            //builder.Services.AddValidatorsFromAssemblyContaining<LoginRequest>();

            ////JWT配置
            //builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JWTSettings>();
            //        byte[] keyBytes = Encoding.UTF8.GetBytes(jwtSettings!.Key);
            //        var secKey = new SymmetricSecurityKey(keyBytes);
            //        opt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
            //            ValidateLifetime = true,
            //            //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
            //            ClockSkew = TimeSpan.FromSeconds(4),
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = secKey,
            //        };
            //    });


            //Identity配置

            //builder.Services.AddDbContext<MyIdentityDbContext>(opt =>
            //{

            //    opt.UseSqlServer("Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
            //});
            builder.Services.AddDataProtection();
            builder.Services.AddIdentityCore<MyUser>(opt =>
            {
                //设置锁定时间10s
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                //设置试错次数3
                opt.Lockout.MaxFailedAccessAttempts = 3;
                //是否必须有小写字符
                opt.Password.RequireLowercase = false;
                //大写
                opt.Password.RequireUppercase = false;
                //长度
                opt.Password.RequiredLength = 6;
                //特殊字符 ! ? @
                opt.Password.RequireNonAlphanumeric = false;

                //重置密码后自动生成一个密码
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

                //邮箱中的链接/验证码
                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

            });

            var idBuilder = new IdentityBuilder(typeof(MyUser), typeof(MyRole), builder.Services);

            idBuilder.AddEntityFrameworkStores<MyIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<MyUser>>()
                .AddRoleManager<RoleManager<MyRole>>();

            ////注入服务
            //builder.Services.AddScoped<IdentityDomainService>();
            //builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
            //builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


            //builder.Services.Configure<MvcOptions>(opt =>
            //{
            //    opt.Filters.Add<UnitOfWorkActionFilter>();
            //});





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseZhzDefault();

            app.MapControllers();

            app.Run();
        }
    }
}
