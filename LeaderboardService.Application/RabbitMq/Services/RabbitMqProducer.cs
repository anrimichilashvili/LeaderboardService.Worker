using LeaderboardService.Application.RabbitMq.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeaderboardService.Application.RabbitMq.Services
{
    public class RabbitMqProducer : IMessageProducer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IConfiguration _configuration;
        public RabbitMqProducer(IRabbitMqConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public void SendMessage<T>(T message)
        {
            var exchangeName = _configuration["SenderLeaderBoardRabbitMq:ExchangeName"];
            var routingKey = _configuration["SenderLeaderBoardRabbitMq:RoutingKey"];
            var queueName = _configuration["SenderLeaderBoardRabbitMq:QueueName"];

            using var channel = _connection.Connection.CreateModel();

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);


            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchangeName, routingKey, false, null, body);
        }
    }
}
