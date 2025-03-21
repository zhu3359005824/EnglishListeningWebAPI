
using FluentValidation.AspNetCore;
using FluentValidation;
using Listening.Domain;
using Listening.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ZHZ.JWT;
using Listening.Admin.WebAPI.Controllers.CategoryController;
using Listening.Admin.WebAPI.Controllers.EpisodeController;
using Listening.Admin.WebAPI.Controllers.AlbumController;
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
            app.MapHub<EpisodeEncodingStatusHub>("/Hubs/EpisodeEncodingStatusHub");

           app.UseZhzDefault();


            app.MapControllers();

            app.Run();
        }
    }
}
