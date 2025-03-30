using ZHZ.Entity;




namespace FileService.Domain.Entity
{
    public class UploadItem : AggregateRootEntity
    {

        public string FileName { get; private set; }

        public string FileSHA256Hash { get; init; }

        public long FileByteSize { get; init; }

        public Uri SourceUrl { get; init; }





        private UploadItem() { }


        public UploadItem(Guid id, DateTime CreateTime, string fileName, string fileSHA256Hash, long fileByteSize, Uri sourceUrl)
        {
            Id = id;
            this.CreateTime = CreateTime;
            FileName = fileName;
            FileSHA256Hash = fileSHA256Hash;
            FileByteSize = fileByteSize;
            SourceUrl = sourceUrl;
        }

        public void ChangeFileName(string fileName)
        {
            FileName = fileName;
        }

    }
}
