using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQTest.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("queue-uri"),
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rawdata", durable: false, exclusive: false, autoDelete: false, arguments: null);
                int repeatCount = 1000;
                while (true)
                {
                    Console.WriteLine($"Press ENTER to send {repeatCount} items to queue");
                    Console.ReadLine();
                    Parallel.For(0, repeatCount, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, (i) =>
                    {
                        string message = "*HQ,861551049378680,V1,120000,A,41.2428,N,36.4133,E,000.00,357.96,150322,ffffffff,(13)(25)(25)(80),(195)(26),00000001,2.78,M1,CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600)-CAN(2;419358976;8;FFFFFFFF3E270600),IMU(-128;-2;-5;-126;-1;-4)#";
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "", routingKey: "ats-rawdata", basicProperties: null, body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    });
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
