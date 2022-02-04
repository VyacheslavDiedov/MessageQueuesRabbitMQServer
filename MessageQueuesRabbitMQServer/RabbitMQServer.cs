using RabbitMQ.Client;
using System;

namespace MessageQueuesRabbitMQServer
{
    internal class RabbitMQServer
    {
        private readonly IModel _connectionFactoryModel;

        public RabbitMQServer()
        {
            _connectionFactoryModel = new ConnectionFactory()
            {
                UserName = "guest",

                Password = "guest",

                HostName = "localhost"

            }.CreateConnection().CreateModel();
        }

        public void InitializeQueue(string exchangeName, string queueName, string directexchangeKey = "directexchange_key")
        {
            CreateExchange(exchangeName);
            CreateQueue(queueName);
            CreateBinding(exchangeName, queueName, directexchangeKey);
        }

        private void CreateExchange(string exchangeName)
        {
            _connectionFactoryModel.ExchangeDeclare(exchangeName, ExchangeType.Direct);


            Console.WriteLine("Created Exchange");
        }

        private void CreateQueue(string queueName)
        {
    
            _connectionFactoryModel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            //_connectionFactoryModel.QueuePurge(queueName);

            Console.WriteLine("Created Exchange");
        }

        private void CreateBinding(string exchangeName, string queueName, string directexchangeKey)
        {
            _connectionFactoryModel.QueueBind(queueName, exchangeName, directexchangeKey);

            Console.WriteLine("Created Binding");
        }

        public BasicGetResult ReadMessage(string queueName)
        {
            Console.WriteLine("Read message");
            return _connectionFactoryModel.BasicGet(queueName, true);
        }

        public void PublishMessage(string exchangeName, byte[] messageBuffer, string directexchangeKey = "directexchange_key")
        {
            //var properties = _connectionFactoryModel.CreateBasicProperties();

            //properties.Persistent = false;

            _connectionFactoryModel.BasicPublish(exchangeName, directexchangeKey, null, messageBuffer);
        }
    }
}
