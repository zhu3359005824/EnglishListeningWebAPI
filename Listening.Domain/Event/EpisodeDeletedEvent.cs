using Listening.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Event
{
    public record EpisodeDeletedEvent(Guid Id) : INotification
    {
    }
}
