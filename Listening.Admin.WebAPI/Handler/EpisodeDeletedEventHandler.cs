﻿using Listening.Domain.Event;
using MediatR;
using ZHZ.EventBus;

namespace Listening.Admin.WebAPI.Handler
{
    public class EpisodeDeletedEventHandler : INotificationHandler<EpisodeDeletedEvent>

    {


        private readonly IEventBus _eventBus;

        public EpisodeDeletedEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(EpisodeDeletedEvent notification, CancellationToken cancellationToken)
        {

            //把领域事件转发为集成事件，让其他微服务听到

            //在领域事件处理中集中进行更新缓存等处理，而不是写到Controller中。因为项目中有可能不止一个地方操作领域对象，这样就就统一了操作。


            var id=notification.Id;

            _eventBus.Publish("ListeningEpisode.Deleted", new {Id=id});
            return Task.CompletedTask;
        }
    }
}
