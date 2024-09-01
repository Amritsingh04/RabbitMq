using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMq_FanOut_Consumer
{
    internal class Program
    {
            static async Task Main(string[] args)
            {
                Console.WriteLine("Fanout Consumer Started");

                var consumer = new Program();
                await consumer.Run();
            }

            public async Task Run()
            {
                // Establish connection and channel
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                string exchangeName = "ProductExchange";
                string[] queueNames = { "Queue1", "Queue2" }; // List of queues bound to the exchange

                channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");
                foreach (var queueName in queueNames)
                {
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
                }

                foreach (var queueName in queueNames)
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"[Fanout] Received message from {queueName}: {message}");
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                }

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
    }

}
