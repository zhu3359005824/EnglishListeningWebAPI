using MediaEncoder.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.Entity;

namespace MediaEncoder.Domain
{
    public class EncodingItem:AggregateRootEntity
    {
        

        /// <summary>
        /// 哪个服务的任务
        /// </summary>
        public string SourceSystem {  get; private set; }   
        
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

       

        public EncodingItem(string sourceSystem, long fileByteSize, string fileName, string fileSHA256Hash, Uri? sourceUrl, Uri? outputUrl, string outType, ItemStatus status, string logText):base()
        {
            SourceSystem = sourceSystem;
            FileByteSize = fileByteSize;
            FileName = fileName;
            FileSHA256Hash = fileSHA256Hash;
            SourceUrl = sourceUrl;
            OutputUrl = outputUrl;
            OutType = outType;
            Status = status;
            LogText = logText;


            this.AddDomainEvent(new EncodingItemCreatedEvent(this.Id, SourceSystem, OutputUrl));
        }

        public void Start()
        {
            this.Status=ItemStatus.Running;
            this.LogText = "正在进行转码";
            //添加事件
            AddDomainEvent(new EncodingItemStartedEvent(this.Id, SourceSystem));


        }


        public void Complete(Uri outputUrl)
        {
            this.OutputUrl=outputUrl;
            this.Status=ItemStatus.Finish;
            this.LogText = "转码成功";

            //添加事件
            AddDomainEvent(new EncodingItemFinishEvent(this.Id,SourceSystem,OutputUrl));
        }

        public void Fail(string logText)
        {
            this.Status=ItemStatus.Failed;
            this.LogText=logText;
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
