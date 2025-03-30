using IDentity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
