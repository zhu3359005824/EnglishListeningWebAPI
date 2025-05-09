
using GlobalConfigurations;

namespace MediaEncoder.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureExtensionService(new InitializerOptions()
            {
                EventBusQueueName = "MediaEncoder.WebAPI",
                LogFilePath = "e:/temp/MediaEncoder.log"
            });
            builder.Services.AddHostedService<EncoderBackgroundService>();//后台转码服务
            builder.Services.AddHttpClient();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
