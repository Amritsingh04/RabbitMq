using RabbitMQ.Client;
using System.Collections.Specialized;
using System.Text;

namespace Product_Producer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var prog = new Program();
            await prog.Run();
        }


        public async Task Run()
        {
            var exchangeType = await ChooseRabbitMqExchangeType();
            Console.WriteLine("Provide Message to send to MicroService");
            var message = Console.ReadLine();
            await CreateConnection(exchangeType, message);
        }

        public async Task<string> ChooseRabbitMqExchangeType()
        {
            Console.WriteLine("Choose Rabbit MQ Exchange Type:\n1. Direct\n2. Fanout");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    return "direct";
                case "2":
                    return "fanout";
                default:
                    Console.WriteLine("Invalid choice. Defaulting to 'direct'.");
                    return "direct";
            }
        }

        public async Task CreateConnection(string exchangeType, string message)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost"
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                // Declare exchange based on the chosen type
                channel.ExchangeDeclare(exchange: "ProductExchange", type: exchangeType);

                var bodyMessage = Encoding.UTF8.GetBytes(message);

                if (exchangeType == "fanout")
                {
                    channel.BasicPublish(exchange: "ProductExchange",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: bodyMessage);
                }
                else if (exchangeType == "direct")
                {
                    channel.BasicPublish(exchange: "ProductExchange",
                                         routingKey: "ProductTest_Queue",
                                         basicProperties: null,
                                         body: bodyMessage);
                }

                Console.WriteLine("Message Sent to Exchange");
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
