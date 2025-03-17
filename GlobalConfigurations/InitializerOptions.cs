using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConfigurations
{/// <summary>
/// 初始化程序选项  日志路径 消息队列名
/// </summary>
    public class InitializerOptions
    {
        public string LogFilePath { get; set; }


        public string EventBusQueueName { get; set; }
    }
}
