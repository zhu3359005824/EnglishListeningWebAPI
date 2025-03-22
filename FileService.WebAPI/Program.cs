
using FileService.Domain;
using FileService.Infrastructure;
using GlobalConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FileService.WebAPI
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

            

            builder.ConfigureExtensionService(new InitializerOptions()
            {
                EventBusQueueName = "1",
                LogFilePath = "E:/Identity.log"
            });
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
