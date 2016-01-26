﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rabbitMQconsole
{
    class Receive
    {
        public static void Main()
        {
                        var factory = new ConnectionFactory() { HostName = "localhost" };
                        using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: "czesc", durable: false, exclusive: false, autoDelete: false, arguments: null);

                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, ea) =>
                            {
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine("wiadomosc przychodzaca: " + message);
                            };
                            channel.BasicConsume(queue: "czesc", noAck: true, consumer: consumer);

                            Console.ReadKey();
            }
        }
    }
}
