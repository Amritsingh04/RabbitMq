using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Rabbit_Mq_Direct
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Direct Consumer Started");

            var consumer = new Program();
            await consumer.Run();
        }

        public async Task Run()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string exchangeName = "ProductExchange";
            var queueBindings = new Dictionary<string, string>
            {
                { "Queue1", "routingKey1" },
                { "Queue2", "routingKey2" }
            };

            channel.ExchangeDeclare(exchange: exchangeName, type: "direct");

            foreach (var (queueName, routingKey) in queueBindings)
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
            }
            foreach (var queueName in queueBindings.Keys)
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"[Direct] Received message from {queueName}: {message}");
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
