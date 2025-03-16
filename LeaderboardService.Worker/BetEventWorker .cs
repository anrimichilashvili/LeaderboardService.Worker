using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using LeaderboardService.Domain.Models;
using LeaderboardService.Application.Interfaces;
using LeaderboardService.Application.RabbitMq.Interfaces; // Adjust namespace if needed

namespace LeaderboardService.Worker
{
    public class BetEventWorker : BackgroundService
    {
        private readonly ILogger<BetEventWorker> _logger;
        private readonly IMessageConsumer _consumer;
        private readonly ILeaderboardService _leaderboardService;

        public BetEventWorker(ILogger<BetEventWorker> logger, IMessageConsumer consumer, ILeaderboardService leaderboardService)
        {
            _logger = logger;
            _consumer = consumer;
            _leaderboardService = leaderboardService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.StartConsumingWithChannel<BetEvent>(betEvent =>
            {
                _leaderboardService.ProcessBet(betEvent);
            });

            return Task.CompletedTask;
        }
    }
}
