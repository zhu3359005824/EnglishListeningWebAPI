using GlobalConfigurations;
using Listening.Admin.WebAPI.Hubs;

namespace Listening.Admin.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddSignalR();
            builder.Services.AddScoped<EncodingEpisodeHelper>();
            builder.ConfigureExtensionService(new InitializerOptions()
            {
                EventBusQueueName = "Listening.Admin",
                LogFilePath = "e:/temp/Listening.Admin.log"
            });








            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapHub<EpisodeEncodingStatusHub>("/Hubs/EpisodeEncodingStatusHub");

            app.UseZhzDefault();


            app.MapControllers();

            app.Run();
        }
    }
}
