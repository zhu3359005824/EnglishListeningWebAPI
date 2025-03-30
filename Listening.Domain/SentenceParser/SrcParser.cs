using Listening.Domain.ValueObject;
using SubtitlesParser.Classes.Parsers;
using System.Text;

namespace Listening.Domain.SentenceParser
{

    // 导包SubtitlesParser.Classes.Parsers;

    public class SrcParser : ISentenceParser
    {
        public bool CanParse(string sentenceType)
        {
            return sentenceType.Equals("src", StringComparison.OrdinalIgnoreCase) ||
                 sentenceType.Equals("vrc", StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<Sentence> Parse(string sentenceContext)
        {
            var srtParser = new SubParser();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sentenceContext)))
            {
                var items = srtParser.ParseStream(ms);
                return items.Select(s => new Sentence(TimeSpan.FromMilliseconds(s.StartTime),
                    TimeSpan.FromMilliseconds(s.EndTime), String.Join(" ", s.Lines)));
            }
        }
    }
}
