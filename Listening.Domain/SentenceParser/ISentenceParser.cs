using Listening.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.SentenceParser
{
    public interface ISentenceParser
    {
        bool CanParse(string sentenceType);

        IEnumerable<Sentence> Parse(string sentenceContext);
    }
}
