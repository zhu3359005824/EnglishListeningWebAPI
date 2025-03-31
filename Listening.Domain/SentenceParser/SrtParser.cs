using Listening.Domain.ValueObject;
using SubtitlesParser.Classes.Parsers;
using System.Text;

namespace Listening.Domain.SentenceParser
{

    // 导包SubtitlesParser.Classes.Parsers;

    public class SrtParser : ISentenceParser
    {
        public bool CanParse(string sentenceType)
        { bool result= sentenceType.Equals("srt", StringComparison.OrdinalIgnoreCase) ||
                 sentenceType.Equals("vrc", StringComparison.OrdinalIgnoreCase);
            return result;
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
