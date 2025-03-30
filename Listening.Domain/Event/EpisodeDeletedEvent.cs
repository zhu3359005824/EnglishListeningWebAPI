using MediatR;

namespace Listening.Domain.Event
{
    public record EpisodeDeletedEvent(Guid Id) : INotification
    {
    }
}
