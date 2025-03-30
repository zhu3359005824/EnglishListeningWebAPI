using Listening.Domain.ValueObject;

namespace Listening.Domain.SentenceParser
{
    public interface ISentenceParser
    {
        bool CanParse(string sentenceType);

        IEnumerable<Sentence> Parse(string sentenceContext);
    }
}
