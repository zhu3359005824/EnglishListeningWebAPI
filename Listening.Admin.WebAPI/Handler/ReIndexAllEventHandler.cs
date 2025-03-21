

using Listening.Domain.Entity;
using Listening.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ZHZ.EventBus;
using ZHZ.EventBus.Handler;

namespace Listening.Admin.WebAPI.EventHandlers
{
    [EventName("SearchService.ReIndexAll")]
    //让搜索引擎服务器，重新收录所有的Episode
    public class ReIndexAllEventHandler : IIntergrationEventHandler
    {
        private readonly ListeningDbContext dbContext;
        private readonly IEventBus eventBus;

        public ReIndexAllEventHandler(ListeningDbContext dbContext, IEventBus eventBus)
        {
            this.dbContext = dbContext;
            this.eventBus = eventBus;
        }

        public Task Handle(string eventName, string eventData)
        {
            foreach (var episode in dbContext.Episodes.AsNoTracking())
            {
                
                    var sentences = episode.GetSentenceContext();
                    eventBus.Publish("ListeningEpisode.Updated", new { Id = episode.Id, episode.EpisodeName, Sentences = sentences, episode.AlbumId, episode.SentenceContxt, episode.SentenceType });
                
            }
            return Task.CompletedTask;
        }
    }
}
