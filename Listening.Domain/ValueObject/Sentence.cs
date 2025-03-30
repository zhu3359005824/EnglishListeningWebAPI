namespace Listening.Domain.ValueObject
{
    public record Sentence(TimeSpan? StartTime, TimeSpan? EndTime, string context)
    {
    }
}
