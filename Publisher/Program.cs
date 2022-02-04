using System.IO;
using System.Threading;

namespace Publisher
{
    class Program
    {
        const string queueName = "myTestQueue";
        const string exchangeName = "myTestExchange";
        const string pathForGetFile = @"C:\Users\Viacheslav_Diedov\Documents\TestOut";
        static void Main(string[] args)
        {
            var rabbitMqPublisher = new RabbitMQPublisher();
            rabbitMqPublisher.InitializeQueue(exchangeName, queueName);

            while (true)
            {
                string[] filesName = Directory.GetFiles(pathForGetFile, "*.pdf");

                foreach (var fileName in filesName)
                {
                    using (FileStream fstream = File.OpenRead(fileName))
                    {
                        byte[] data = new byte[fstream.Length];
                        fstream.Read(data, 0, data.Length);

                        rabbitMqPublisher.PublishMessage(exchangeName, data);
                    }
                    File.Delete(fileName);
                }
                Thread.Sleep(10000);
            }
        }
    }
}
