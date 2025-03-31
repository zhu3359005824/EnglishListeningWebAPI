using MediaEncoder.Domain.Events;
using ZHZ.Entity;

namespace MediaEncoder.Domain
{
    public class EncodingItem : AggregateRootEntity
    {


        /// <summary>
        /// 哪个服务的任务
        /// </summary>
        public string SourceSystem { get; private set; }

        public long FileByteSize { get; private set; }

        public string FileName { get; private set; }


        public string FileSHA256Hash { get; private set; }


        public Uri SourceUrl { get; private set; }

        public Uri OutputUrl { get; private set; }

        /// <summary>
        /// 转码类型
        /// </summary>
        public string OutType { get; private set; }

        public ItemStatus Status { get; private set; }

        public string LogText { get; private set; }

        private EncodingItem() { }




        public static EncodingItem Create(Guid id, string name, string outputType, string sourceSystem)
        {
            EncodingItem item = new EncodingItem()
            {
                Id = id,
                CreateTime = DateTime.Now,
                FileName = name,
                OutType = outputType,
                
                Status = ItemStatus.Ready,
                SourceSystem = sourceSystem,
            };
            item.AddDomainEvent(new EncodingItemCreatedEvent(item));
            return item;
        }

        public void Start()
        {
            this.Status = ItemStatus.Started;
            this.LogText = "正在进行转码";
            //添加事件
            AddDomainEvent(new EncodingItemStartedEvent(this.Id, SourceSystem));


        }


        public void Complete(Uri outputUrl)
        {
            this.OutputUrl = outputUrl;
            this.Status = ItemStatus.Completed;
            this.LogText = "转码成功";

            //添加事件
            AddDomainEvent(new EncodingItemFinishEvent(this.Id, SourceSystem, OutputUrl));
        }

        public void Fail(string logText)
        {
            this.Status = ItemStatus.Failed;
            this.LogText = logText;
            //添加事件
            AddDomainEvent(new EncodingItemFailedEvent(this.Id, SourceSystem, logText));
        }

        public void Fail(Exception exception)
        {
            Fail($"转码失败{exception}");
        }

        public void ChangeFileMeta(long fileSize, string srcFileHash)
        {
            this.FileByteSize = fileSize;
            this.FileSHA256Hash = srcFileHash;
        }
    }
}
