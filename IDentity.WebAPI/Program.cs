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

            ////JWT����
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
            //            // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
            //            ValidateLifetime = true,
            //            //ע�����ǻ������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬��������ã�Ĭ����5����
            //            ClockSkew = TimeSpan.FromSeconds(4),
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = secKey,
            //        };
            //    });


            //Identity����

            //builder.Services.AddDbContext<MyIdentityDbContext>(opt =>
            //{

            //    opt.UseSqlServer("Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
            //});
            builder.Services.AddDataProtection();
            builder.Services.AddIdentityCore<MyUser>(opt =>
            {
                //��������ʱ��10s
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                //�����Դ����3
                opt.Lockout.MaxFailedAccessAttempts = 3;
                //�Ƿ������Сд�ַ�
                opt.Password.RequireLowercase = false;
                //��д
                opt.Password.RequireUppercase = false;
                //����
                opt.Password.RequiredLength = 6;
                //�����ַ� ! ? @
                opt.Password.RequireNonAlphanumeric = false;

                //����������Զ�����һ������
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

                //�����е�����/��֤��
                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

            });

            var idBuilder = new IdentityBuilder(typeof(MyUser), typeof(MyRole), builder.Services);

            idBuilder.AddEntityFrameworkStores<MyIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<MyUser>>()
                .AddRoleManager<RoleManager<MyRole>>();

            ////ע�����
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
