using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDentity.Infrastructure
{
    internal class MyDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyIdentityDbContext>
    {
        public MyIdentityDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MyIdentityDbContext> builder = new DbContextOptionsBuilder<MyIdentityDbContext>();

            string conStr = "Server=.;Database=EnglishListeningWeb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
            builder.UseSqlServer(conStr);
            MyIdentityDbContext myDbContext = new MyIdentityDbContext(builder.Options);

            return myDbContext;
        }
    }
}
