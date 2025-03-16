using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading.Channels;
using LeaderboardService.Domain.Models;
using LeaderboardService.Application.RabbitMq.Interfaces;

namespace LeaderboardService.Application.RabbitMq.Services
{
    public class RabbitMqConsumer : IMessageConsumer, IDisposable
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IConfiguration _configuration;
        private IModel _channel;
        private string _exchangeName;
        private string _routingKey;
        private string _queueName;

        public RabbitMqConsumer(IRabbitMqConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
            InitializeConsumer();
        }

        private void InitializeConsumer()
        {
            _exchangeName = _configuration["ReceiverFromHubServiceRabbitMq:ExchangeName"];
            _routingKey = _configuration["ReceiverFromHubServiceRabbitMq:RoutingKey"];
            _queueName = _configuration["ReceiverFromHubServiceRabbitMq:QueueName"];

            _channel = _connection.Connection.CreateModel();


            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(_queueName, false, false, false, null);
            _channel.QueueBind(_queueName, _exchangeName, _routingKey, null);
            _channel.BasicQos(0, 1, false);
        }

        public void StartConsumingWithChannel<T>(Action<T> onMessage)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);
                    T messageObj = JsonSerializer.Deserialize<T>(json);

                    onMessage(messageObj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
                finally
                {
                    _channel.BasicAck(args.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(_queueName, autoAck: false, consumer: consumer);
        }
        public void Dispose()
        {
            _channel?.Close();
        }
    }
}
