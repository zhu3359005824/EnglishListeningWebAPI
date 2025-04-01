using Listening.Admin.WebAPI.Hubs;
using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;
using MediaEncoder.Domain.Events;
using MediaEncoder.WebAPI;
using Microsoft.AspNetCore.SignalR;
using ZHZ.EventBus;
using ZHZ.EventBus.Handler;

namespace Listening.Admin.WebAPI.Handler
{
    //收听转码服务发出的集成事件
    //把状态通过SignalR推送给客户端，从而显示“转码进度”
    [EventName("MediaEncoding.Started")]
    [EventName("MediaEncoding.Failed")]
    [EventName("MediaEncoding.Duplicated")]
    [EventName("MediaEncoding.Completed")]
    public class EncodingStatusChangeIntergrationHandler : DynamicIntegrationEventHandler
    {

        private readonly ListeningDbContext _context;
        private readonly IListeningRepository _repository;
        private readonly EncodingEpisodeHelper _episodeHelper;
        private readonly IHubContext<EpisodeEncodingStatusHub> _hubContext;

        public EncodingStatusChangeIntergrationHandler(ListeningDbContext context, IListeningRepository repository, EncodingEpisodeHelper episodeHelper, IHubContext<EpisodeEncodingStatusHub> hubContext)
        {
            _context = context;
            _repository = repository;
            _episodeHelper = episodeHelper;
            _hubContext = hubContext;
        }

        public override async Task HandleEncodingItem(string eventName, object eventData)
        {


            switch (eventName)
            {
                case "MediaEncoding.Started":
                    var start = eventData as EncodingItemStartedEvent;

                    await _episodeHelper.UpdateEpisodeStatusAsync(start.FileName, "Started");
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingStarted", start.FileName);//通知前端刷新
                    break;
                case "MediaEncoding.Failed":
                    var failed = eventData as EncodingItemFailedEvent;
                    await _episodeHelper.UpdateEpisodeStatusAsync(failed.FileName, "Failed");
                    //todo: 这样做有问题，这样就会把消息发送给所有打开这个界面的人，应该用connectionId、userId等进行过滤，
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingFailed", failed.FileName);
                    break;
                case "MediaEncoding.Duplicated":
                    var duplicated = eventData as DuplicateData;
                    await _episodeHelper.UpdateEpisodeStatusAsync(duplicated.FileName, "Completed");
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", duplicated.FileName);//通知前端刷新
                    break;
                case "MediaEncoding.Completed":
                    var completed = eventData as EncodingItemFinishEvent;
                 
                    //转码完成，则从Redis中把暂存的Episode信息取出来，然后正式地插入Episode表中
                    await _episodeHelper.UpdateEpisodeStatusAsync(completed.FileName, "Completed");
                    //Uri outputUrl = new Uri(eventData.OutputUrl);
                    var encItem = await _episodeHelper.GetEncodingEpisodeAsync(completed.FileName);

                  
                    int maxSeq = await _repository.GetMaxIndexOfEpisodesAsync(encItem.AlbumName);
                    /*
                    Episode episode = Episode.Create(id, maxSeq.Value + 1, encodingEpisode.Name, albumId, outputUrl,
                        encodingEpisode.DurationInSecond, encodingEpisode.SubtitleType, encodingEpisode.Subtitle);*/


                    Episode episode = new Episode(encItem.AlbumName, encItem.SentenceContext, encItem.SentenceType, encItem.EpisodeName, completed.OutputUrl);
                    _context.Add(episode);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", completed.FileName);//通知前端刷新
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventName));
            }
        }
    }
}
