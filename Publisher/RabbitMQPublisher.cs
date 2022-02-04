using RabbitMQ.Client;
using System;

namespace Publisher
{
    public class RabbitMQPublisher
    {
        private readonly IModel _connectionFactoryModel;

        public RabbitMQPublisher()
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

            Console.WriteLine("Created queue");
        }

        private void CreateBinding(string exchangeName, string queueName, string directexchangeKey)
        {
            _connectionFactoryModel.QueueBind(queueName, exchangeName, directexchangeKey);

            Console.WriteLine("Created Binding");
        }

        public void PublishMessage(string exchangeName, byte[] messageBuffer, string directexchangeKey = "directexchange_key")
        {
            _connectionFactoryModel.BasicPublish(exchangeName, directexchangeKey, null, messageBuffer);

            Console.WriteLine("Message sent");
        }
    }
}
