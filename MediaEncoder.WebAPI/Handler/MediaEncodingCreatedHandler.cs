

using MediaEncoder.Domain;
using MediaEncoder.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZHZ.EventBus;
using ZHZ.EventBus.Handler;

namespace MediaEncoder.WebAPI.EventHandlers;
/// <summary>
/// 保存转码任务到T_EncodingItems表中
/// </summary>
[EventName("MediaEncoding.Created")]
public class MediaEncodingCreatedHandler : DynamicIntegrationEventHandler
{
    private readonly IEventBus eventBus;
    private readonly MediaEncoderDbContext dbContext;

    public MediaEncodingCreatedHandler(IEventBus eventBus, MediaEncoderDbContext dbContext)
    {
        this.eventBus = eventBus;
        this.dbContext = dbContext;
    }

    public override async Task HandleEncodingItem(string eventName, object eventData)
    {
       
        var data = JsonSerializer.Deserialize<MediaEncodingData>(eventData.ToString());
        Guid mediaId = data.Id;
        string sourceSystem = data.SourceSystem;
        string episodeName = data.FileName;
        string outputType = data.OutputType;
        Uri srcUrl=data.SourceUrl;
        //保证幂等性，如果这个路径对应的操作已经存在，则直接返回
        bool exists = await dbContext.EncodingItems
            .AnyAsync(e => e.FileName == episodeName && e.OutType == outputType);
        if (exists)
        {
            return;
        }

        //把任务插入数据库，也可以看作是一种事件，不一定非要放到MQ中才叫事件
        //没有通过领域事件执行，因为如果一下子来很多任务，领域事件就会并发转码，而这种方式则会一个个的转码
        //直接用另一端传来的MediaId作为EncodingItem的主键
        var encodeItem = EncodingItem.Create(mediaId, episodeName, outputType, sourceSystem,data.FileSHA256Hash,data.FileByteSize,srcUrl);

        dbContext.Add(encodeItem);
        await dbContext.SaveChangesAsync();
    }
    
}
