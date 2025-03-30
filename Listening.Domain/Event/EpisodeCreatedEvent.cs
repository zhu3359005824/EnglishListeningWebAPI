using Listening.Domain.Entity;
using MediatR;

namespace Listening.Domain.Event
{
    public record EpisodeCreatedEvent(Episode Value) : INotification
    {
    }
}
