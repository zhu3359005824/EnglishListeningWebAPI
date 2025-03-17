using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.Entity;




namespace FileService.Domain.Entity
{
    public class UploadItem :AggregateRootEntity
    {
        public Guid Id { get; init; }
        public string FileName { get; private set; }

        public string FileSHA256Hash { get; init; }

        public long FileByteSize { get; init; }
        public DateTime CreateTime { get; init; }




        private UploadItem() { }


        public UploadItem(Guid id, DateTime CreateTime, string fileName, string fileSHA256Hash, long fileByteSize)
        {
            Id = id;
            this.CreateTime = CreateTime;
            FileName = fileName;
            FileSHA256Hash = fileSHA256Hash;
            FileByteSize = fileByteSize;
          
        }

        public void ChangeFileName(string fileName)
        {
            FileName = fileName;
        }

    }
}
