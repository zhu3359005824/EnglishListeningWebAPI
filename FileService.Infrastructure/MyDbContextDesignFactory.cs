using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FileService.Infrastructure
{
    internal class MyDbContextDesignFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MyDbContext> builder = new DbContextOptionsBuilder<MyDbContext>();

            string conStr = "Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
            builder.UseSqlServer(conStr);
            MyDbContext myDbContext = new MyDbContext(builder.Options);

            return myDbContext;
        }
    }
}
