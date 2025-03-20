using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.EventBus.Handler
{/// <summary>
/// 集成事件处理器实现的接口
/// </summary>
    public interface IIntergrationEventHandler
    {
        //因为消息可能会重复发送，因此Handle内的实现需要是幂等的
        Task Handle(string eventName, string eventData);
    }
}
