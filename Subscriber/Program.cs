using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;

namespace Subscriber
{
    class Program
    {
        const string queueName = "myTestQueue";
        const string pathForPasteFile = @"C:\Users\Viacheslav_Diedov\Documents\TestIn\";

        static void Main(string[] args)
        {
            int count = 1;
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, data) =>
                {
                    File.WriteAllBytes(pathForPasteFile + $"file{count}.pdf", data.Body.ToArray());
                    Console.WriteLine($"File {count++} created");
                };

                channel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine("Subscribe to queue");
                Console.ReadKey();
            }
        }
    }
}
