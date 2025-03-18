using MediaEncoder.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure
{
    internal class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MediaEncoderDbContext>
    {
        public MediaEncoderDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MediaEncoderDbContext> builder = new DbContextOptionsBuilder<MediaEncoderDbContext>();

            string conStr = "Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
            builder.UseSqlServer(conStr);
            MediaEncoderDbContext myDbContext = new MediaEncoderDbContext(builder.Options);

            return myDbContext;
        }
    }
}
