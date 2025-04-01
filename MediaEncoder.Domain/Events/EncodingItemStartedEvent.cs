using MediatR;

namespace MediaEncoder.Domain.Events
{
    public record EncodingItemStartedEvent(Guid id, string SourceSystem,string FileName) : INotification
    {
    }
}
