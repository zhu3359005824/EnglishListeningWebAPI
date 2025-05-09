﻿using IDentity.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IDentity.Infrastructure
{
    public class MyIdentityDbContext : IdentityDbContext<MyUser, MyRole, Guid>
    {
        public DbSet<MyUser> Users { get; set; }
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

    }
}
