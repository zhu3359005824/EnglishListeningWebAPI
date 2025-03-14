using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
