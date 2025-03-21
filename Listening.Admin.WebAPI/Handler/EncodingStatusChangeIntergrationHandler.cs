using Listening.Admin.WebAPI.Hubs;
using Listening.Domain;
using Listening.Domain.Entity;
using Listening.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ZHZ.EventBus;
using ZHZ.EventBus.Handler;

namespace Listening.Admin.WebAPI.Handler
{
    //收听转码服务发出的集成事件
    //把状态通过SignalR推送给客户端，从而显示“转码进度”
    [EventName("MediaEncoding.Prepared")]
    [EventName("MediaEncoding.Running")]
    [EventName("MediaEncoding.Finish")]
    [EventName("MediaEncoding.Failed")]
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

        public override  async Task HandleDynamic(string eventName, dynamic eventData)
        {
            string sourceSystem = eventData.SourceSystem;
            if (sourceSystem != "Listening")//可能是别的系统的转码消息
            {
                return;
            }
            string episodeName = eventData.EpisodeName;//EncodingItem的Id就是Episode 的Id

            switch (eventName)
            {
                case "MediaEncoding.Started":
                    await _episodeHelper.UpdateEpisodeStatusAsync(episodeName, "Started");
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingStarted", episodeName);//通知前端刷新
                    break;
                case "MediaEncoding.Failed":
                    await _episodeHelper.UpdateEpisodeStatusAsync(episodeName, "Failed");
                    //todo: 这样做有问题，这样就会把消息发送给所有打开这个界面的人，应该用connectionId、userId等进行过滤，
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingFailed", episodeName);
                    break;
                case "MediaEncoding.Duplicated":
                    await _episodeHelper.UpdateEpisodeStatusAsync(episodeName, "Completed");
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", episodeName);//通知前端刷新
                    break;
                case "MediaEncoding.Completed":
                    //转码完成，则从Redis中把暂存的Episode信息取出来，然后正式地插入Episode表中
                    await _episodeHelper.UpdateEpisodeStatusAsync(episodeName, "Completed");
                    //Uri outputUrl = new Uri(eventData.OutputUrl);
                    var encItem = await _episodeHelper.GetEncodingEpisodeAsync(episodeName);

                    Guid albumId = encItem.AlbumId;
                    int maxSeq = await _repository.GetMaxIndexOfEpisodesAsync(albumId);
                    /*
                    Episode episode = Episode.Create(id, maxSeq.Value + 1, encodingEpisode.Name, albumId, outputUrl,
                        encodingEpisode.DurationInSecond, encodingEpisode.SubtitleType, encodingEpisode.Subtitle);*/
                    

                    Episode episode = new Episode(albumId, encItem.SentenceContext,encItem.SentenceType,encItem.EpisodeName);
                    _context.Add(episode);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("OnMediaEncodingCompleted", episodeName);//通知前端刷新
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventName));
            }
        }
    }
}
