using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain.Events
{
    public record EncodingItemCreatedEvent(EncodingItem EncodingItem):INotification
    {
    }
}
