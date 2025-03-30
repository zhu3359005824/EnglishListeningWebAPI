using MediatR;

namespace MediaEncoder.Domain.Events
{
    public record EncodingItemCreatedEvent(EncodingItem EncodingItem) : INotification
    {
    }
}
