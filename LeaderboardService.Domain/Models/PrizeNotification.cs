using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardService.Domain.Models
{
    public class PrizeNotification
    {
        public string CreatedBy { get; set; }
        public decimal Prize { get; set; }
    }
}
