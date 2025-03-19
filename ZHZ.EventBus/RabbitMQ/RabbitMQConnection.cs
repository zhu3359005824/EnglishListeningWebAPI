using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.EventBus.RabbitMQ
{
    internal class RabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;

       

        private readonly IConnection _connection;

        private bool _disposed;

        private readonly object sync_root = new object();

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection=  _connectionFactory.CreateConnectionAsync().Result;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public Task<IChannel> CreateChannel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateChannelAsync();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _connection.Dispose();
        }

       

      
    }
}
