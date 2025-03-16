using LeaderboardService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardService.Application.Interfaces
{
    public interface ILeaderboardService
    {
        void ProcessBet(BetEvent betEvent);
        IEnumerable<LeaderboardEntry> GetTopPlayers(int count);
        void Reset();
    }
}
