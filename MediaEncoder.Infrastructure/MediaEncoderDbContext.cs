﻿using MediaEncoder.Domain;
using Microsoft.EntityFrameworkCore;

namespace MediaEncoder.Infrastructure
{
    public class MediaEncoderDbContext : DbContext
    {
        public DbSet<EncodingItem> EncodingItems { get; set; }

        public MediaEncoderDbContext(DbContextOptions<MediaEncoderDbContext> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
