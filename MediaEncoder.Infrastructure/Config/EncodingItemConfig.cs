using MediaEncoder.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Infrastructure.Config
{
    public class EncodingItemConfig : IEntityTypeConfiguration<EncodingItem>
    {
        public void Configure(EntityTypeBuilder<EncodingItem> builder)
        {
            builder.ToTable("T_EncodingItems");

            builder.HasKey(x => x.Id).IsClustered(false);
            //讲枚举值的字符串保存
            builder.Property(e => e.Status).HasConversion<string>();



            builder.Property(e => e.FileSHA256Hash).HasMaxLength(64).IsUnicode(false);
            builder.Property(e => e.OutType).HasMaxLength(10).IsUnicode(false);

            builder.HasQueryFilter(e => e.IsDeleted == false);


        }
    }
}
