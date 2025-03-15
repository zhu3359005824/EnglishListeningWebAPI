using Listening.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure.Config
{
    internal class EpisodeConfig : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.ToTable("T_Episodes");
            builder.HasKey(e => e.Id).IsClustered(false);//Guid类型不要聚集索引，否则会影响性能
            builder.HasIndex(e => new { e.AlbumId, e.IsDeleted });//索引不要忘了加上IsDeleted，否则会影响性能

            builder.HasQueryFilter(e => e.IsDeleted);

            builder.Property(e => e.SentenceContxt).HasMaxLength(int.MaxValue).IsUnicode().IsRequired();
            builder.Property(e => e.SentenceType).HasMaxLength(10).IsUnicode(false).IsRequired();
        }
    }
}
