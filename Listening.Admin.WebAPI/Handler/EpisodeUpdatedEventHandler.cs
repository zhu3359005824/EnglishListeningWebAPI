using Listening.Domain.Event;
using MediatR;
using ZHZ.EventBus;

namespace Listening.Admin.WebAPI.Handler
{
    public class EpisodeUpdatedEventHandler : INotificationHandler<EpisodeUpdatedEvent>

    {


        private readonly IEventBus _eventBus;

        public EpisodeUpdatedEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(EpisodeUpdatedEvent notification, CancellationToken cancellationToken)
        {

            //把领域事件转发为集成事件，让其他微服务听到

            //在领域事件处理中集中进行更新缓存等处理，而不是写到Controller中。因为项目中有可能不止一个地方操作领域对象，这样就就统一了操作。


            var episode = notification.Value;

            var sentences = episode.GetSentenceContext();


            //发布集成事件，实现搜索索引、记录日志等功能
            _eventBus.Publish("ListeningEpisode.Updated", new
            {
                Id = episode.Id,
                episode.EpisodeName,
                Sentences = sentences,
                episode.AlbumName,
                episode.SentenceContxt,
                episode.SentenceType
            });

            return Task.CompletedTask;
        }
    }
}
