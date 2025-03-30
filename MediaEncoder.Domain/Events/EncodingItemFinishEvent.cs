using MediatR;

namespace MediaEncoder.Domain.Events
{
    public record EncodingItemFinishEvent(Guid Id, string SourceSystem, Uri OutputUrl) : INotification
    {
    }
}
