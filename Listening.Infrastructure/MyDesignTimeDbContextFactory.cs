using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Listening.Infrastructure
{
    internal class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ListeningDbContext>
    {
        public ListeningDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ListeningDbContext> builder = new DbContextOptionsBuilder<ListeningDbContext>();

            string conStr = "Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
            builder.UseSqlServer(conStr);
            ListeningDbContext myDbContext = new ListeningDbContext(builder.Options);

            return myDbContext;
        }
    }
}
