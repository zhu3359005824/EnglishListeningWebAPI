using Listening.Domain.Entity;
using MediatR;

namespace Listening.Domain.Event
{
    public record EpisodeUpdatedEvent(Episode Value) : INotification
    {
    }
}
