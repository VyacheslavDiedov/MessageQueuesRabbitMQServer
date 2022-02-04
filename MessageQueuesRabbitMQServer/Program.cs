using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;

namespace MessageQueuesRabbitMQServer
{
    class Program
    {
        const string queueName = "myTestQueue";
        const string exchangeName = "myTestExchange";
        const string pathForGetFile = @"C:\Users\Viacheslav_Diedov\Documents\TestOut";
        const string pathForPasteFile = @"C:\Users\Viacheslav_Diedov\Documents\TestIn";
        static void Main(string[] args)
        {
            var rabbitMqServer = new RabbitMQServer();
            rabbitMqServer.InitializeQueue(exchangeName, queueName);

            while (true)
            {
                string[] filesName = Directory.GetFiles(pathForGetFile, "*.pdf");

                foreach (var fileName in filesName)
                {
                    using (FileStream fstream = File.OpenRead(fileName))
                    {
                        byte[] array = new byte[fstream.Length];
                        fstream.Read(array, 0, array.Length);

                        rabbitMqServer.PublishMessage(exchangeName, array);
                    }
                }


                var temp = rabbitMqServer.ReadMessage(queueName).MessageCount;

                for (int i = 0; i < temp; i++)
                {
                    File.WriteAllBytes($@"{pathForPasteFile}/file{i}.pdf", rabbitMqServer.ReadMessage(queueName).Body.ToArray());
                }

                
            }

            
            


            Console.ReadLine();
        }
    }
}
