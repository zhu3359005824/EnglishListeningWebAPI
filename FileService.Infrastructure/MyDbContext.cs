using FileService.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure
{
    public class MyDbContext : DbContext
    {
        public DbSet<UploadItem> UploadItems { get; set; }


        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
