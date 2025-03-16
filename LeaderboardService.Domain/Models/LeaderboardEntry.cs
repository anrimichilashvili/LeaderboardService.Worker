using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardService.Domain.Models
{
    public class LeaderboardEntry
    {
        public string CreatedBy { get; set; }
        public decimal TotalBet { get; set; }
    }
}
