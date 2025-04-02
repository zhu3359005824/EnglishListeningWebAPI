namespace ZHZ.EventBus
{
    public class MediaEncodingData
    {
        public MediaEncodingData(Guid id, string sourceSystem, string fileName, string outputType, string fileSHA256Hash, long fileByteSize,Uri sourceUrl)
        {
            Id = id;
            SourceSystem = sourceSystem;
            FileName = fileName;
            OutputType = outputType;
            FileSHA256Hash = fileSHA256Hash;
            FileByteSize = fileByteSize;
            SourceUrl = sourceUrl;
        }

        public Guid Id { get; set; }
        public string SourceSystem { get; set; }
        public string FileName { get; set; }
        public string OutputType { get; set; }

        public string FileSHA256Hash { get; init; }

        public long FileByteSize { get; init; }

        public Uri SourceUrl { get; set; }

    }
}
