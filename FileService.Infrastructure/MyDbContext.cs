﻿using FileService.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.Infrastructure.EFCore;

namespace FileService.Infrastructure
{
    public class MyDbContext:DbContext
    {
       public DbSet<UploadItem> UploadItems {  get; set; }
       

       public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
