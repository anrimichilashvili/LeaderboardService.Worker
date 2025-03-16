using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeaderboardService.Domain.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LeaderboardService.Application.Interfaces;
using LeaderboardService.Application.RabbitMq.Interfaces;

namespace LeaderboardService.Worker
{
    public class LeaderboardScheduler : BackgroundService
    {
        private readonly ILogger<LeaderboardScheduler> _logger;
        private readonly ILeaderboardService _leaderboardService;
        private readonly IMessageProducer _messageProducer; 

        public LeaderboardScheduler(
            ILogger<LeaderboardScheduler> logger,
            ILeaderboardService leaderboardService,
            IMessageProducer messageProducer)
        {
            _logger = logger;
            _leaderboardService = leaderboardService;
            _messageProducer = messageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextHour = now.AddHours(1).Date.AddHours(now.Hour + 1);
                var delay = nextHour - now;

                delay = TimeSpan.FromSeconds(10);


                _logger.LogInformation("Next leaderboard computation in {Minutes} minutes", delay.TotalMinutes);
                await Task.Delay(delay, stoppingToken);

                var topPlayers = _leaderboardService.GetTopPlayers(3).ToList();
                if (!topPlayers.Any())
                {
                    _logger.LogInformation("No bets found for this hour.");
                    continue;
                }

                foreach (var entry in topPlayers)
                {
                    decimal prizeAmount = entry.TotalBet * 1.3m;
                    _logger.LogInformation("Player {PlayerId} is awarded {Prize}", entry.CreatedBy, prizeAmount);

                    var notification = new PrizeNotification
                    {
                        CreatedBy = entry.CreatedBy,
                        Prize = prizeAmount
                    };
                    _messageProducer.SendMessage(notification);
                }

                _leaderboardService.Reset();
            }
        }
    }
}
