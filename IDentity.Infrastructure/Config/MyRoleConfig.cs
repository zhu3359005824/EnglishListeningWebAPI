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
    internal class MyRoleConfig : IEntityTypeConfiguration<MyRole>
    {
        public void Configure(EntityTypeBuilder<MyRole> builder)
        {
            builder.ToTable("T_Roles");
        }
    }
}
