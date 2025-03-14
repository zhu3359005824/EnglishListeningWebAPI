using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.ValueObject
{
    public record Sentence(TimeSpan? StartTime, TimeSpan? EndTime,string context)
    {
    }
}
