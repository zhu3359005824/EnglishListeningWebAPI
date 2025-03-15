using Listening.Domain;
using Listening.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Listening.Main.WebAPI
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

            builder.Services.AddDbContext<ListeningDbContext>(opt =>
            {
                opt.UseSqlServer("Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
            });


            builder.Services.AddMemoryCache();


            builder.Services.AddScoped<IListeningRepository , ListeningRepository>();
            builder.Services.AddScoped<ListeningDomainService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
