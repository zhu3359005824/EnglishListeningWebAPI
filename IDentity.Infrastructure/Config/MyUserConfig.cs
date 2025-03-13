using IDentity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDentity.Infrastructure.Config
{
    internal class MyUserConfig : IEntityTypeConfiguration<MyUser>
    {
        public void Configure(EntityTypeBuilder<MyUser> builder)
        {
            builder.ToTable("T_Users");
            builder.HasQueryFilter(b=>b.IsDeleted==false);
        }
    }
}
