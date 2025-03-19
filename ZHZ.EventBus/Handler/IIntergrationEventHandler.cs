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
        Task Handle(string eventName, string eventData);
    }
}
