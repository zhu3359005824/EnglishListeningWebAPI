using Listening.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Listening.Infrastructure.Config
{
    internal class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("T_Categories");
            builder.HasKey(e => e.Id).IsClustered(false);

            builder.Property(e => e.CoverUrl).IsRequired(false).HasMaxLength(500).IsUnicode();
            builder.HasQueryFilter(e => e.IsDeleted == false);
        }
    }
}
