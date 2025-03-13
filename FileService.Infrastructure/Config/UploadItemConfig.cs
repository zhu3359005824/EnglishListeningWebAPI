using FileService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Config
{
    internal class UploadItemConfig : IEntityTypeConfiguration<UploadItem>
    {
        public void Configure(EntityTypeBuilder<UploadItem> builder)
        {
            builder.ToTable("T_UploadItems");
            builder.HasKey(e => e.Id).IsClustered(false);
            builder.HasIndex(e => new { e.FileSHA256Hash, e.FileByteSize });//经常要按照这两个列进行查询，因此把它们两个组成复合索引，提高查询效率。

        }
    }
}
