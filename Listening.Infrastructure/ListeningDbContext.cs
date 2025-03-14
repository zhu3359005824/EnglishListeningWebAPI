using Listening.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure
{
    public class ListeningDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; } 

        public DbSet<Album> Albums { get; set; }

        public DbSet<Episode> Episodes { get; set; }



        public ListeningDbContext(DbContextOptions<ListeningDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
