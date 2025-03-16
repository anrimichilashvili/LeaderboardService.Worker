using LeaderboardService.Application.Interfaces;
using LeaderboardService.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardService.Application.Services
{
    public class InMemoryLeaderboardService : ILeaderboardService
    {
        private readonly ConcurrentDictionary<string, decimal> _playerTotals = new();

        public void ProcessBet(BetEvent betEvent)
        {
            _playerTotals.AddOrUpdate(betEvent.CreatedBy, betEvent.Amount, (key, current) => current + betEvent.Amount);
        }

        public IEnumerable<LeaderboardEntry> GetTopPlayers(int count)
        {
            return _playerTotals
                .OrderByDescending(x => x.Value)
                .Take(count)
                .Select(x => new LeaderboardEntry { CreatedBy = x.Key, TotalBet = x.Value });
        }

        public void Reset()
        {
            _playerTotals.Clear();
        }
    }
}
