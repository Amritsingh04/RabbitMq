Overview
This repository demonstrates how to use RabbitMQ for message queuing with different exchange types. RabbitMQ is a robust messaging broker that facilitates communication between applications through message queuing. This example includes a producer and two types of consumers (fanout and direct), showcasing how messages can be routed and consumed based on exchange types.

RabbitMQ Basics
RabbitMQ is an open-source message broker that implements the Advanced Message Queuing Protocol (AMQP). It allows applications to communicate asynchronously by sending messages to queues that other applications can read from. RabbitMQ uses exchanges to route messages to one or more queues based on routing rules.

Exchanges
Fanout Exchange:

Routes messages to all queues bound to the exchange.
Useful for broadcasting messages to multiple queues.
No routing key is used; all messages are delivered to all bound queues.
Direct Exchange:

Routes messages to queues based on a specific routing key.
Queues need to be bound with the same routing key used when publishing the message.
Useful for routing messages to specific queues based on predefined criteria.
Setup and Usage
Docker Image for RabbitMQ
We use a Docker image to set up RabbitMQ. To run RabbitMQ locally using Docker, execute the following command:

bash
Copy code
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
-p 5672:5672 exposes the RabbitMQ service port.
-p 15672:15672 exposes the RabbitMQ management interface.
You can access the RabbitMQ management interface at http://localhost:15672/ using the default credentials:

Username: guest
Password: guest
Producer
The producer application sends messages to RabbitMQ. It allows you to choose between a fanout or direct exchange and sends messages accordingly.

Fanout Exchange Producer
In the FanoutProducer code:

Declares a fanout exchange.
Binds multiple queues to this exchange.
Sends messages to the exchange, which are then broadcast to all bound queues.
Direct Exchange Producer
In the DirectProducer code:

Declares a direct exchange.
Binds multiple queues to this exchange with specific routing keys.
Sends messages to the exchange with a routing key that determines which queues receive the message.
Consumers
Consumers are applications that receive messages from RabbitMQ queues. We have two types of consumers based on the exchange types:

Fanout Consumer
In the FanoutConsumer code:

Connects to a fanout exchange.
Binds to multiple queues that are all connected to the same fanout exchange.
Receives and processes messages broadcast by the exchange.
Direct Consumer
In the DirectConsumer code:

Connects to a direct exchange.
Binds to queues with specific routing keys.
Receives and processes messages routed based on the routing keys.
Running the Applications
Start RabbitMQ using the Docker command provided above.
Run the Producer:
Navigate to the producer’s directory and execute the application.
Run the Consumers:
Navigate to each consumer’s directory and execute the application.
Example Commands
To run the producers and consumers, use commands similar to these:

bash
Copy code
dotnet run --project path\to\FanoutProducer
dotnet run --project path\to\DirectProducer
dotnet run --project path\to\FanoutConsumer
dotnet run --project path\to\DirectConsumer
Replace path\to\ with the actual path to the respective projects.

Conclusion
This example illustrates how RabbitMQ can be used to route and process messages with different exchange types. By using Docker for RabbitMQ setup, it simplifies the deployment and configuration process. For more information, refer to the RabbitMQ Documentation.
