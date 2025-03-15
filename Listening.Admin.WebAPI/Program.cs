
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

            //添加验证规则
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<AddCategoryRequest>();
            builder.Services.AddValidatorsFromAssemblyContaining<AddEpisodeRequest>();
            builder.Services.AddValidatorsFromAssemblyContaining<AddAlbumRequest>();


            builder.Services.AddDbContext<ListeningDbContext>(opt =>
            {
                opt.UseSqlServer("Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
            });

            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IListeningRepository, ListeningRepository>();
            builder.Services.AddScoped<ListeningDomainService>();



         


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
