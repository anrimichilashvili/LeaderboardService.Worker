using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardService.Domain.Models
{
    public class BetEvent
    {
        public decimal Amount { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public int Id { get; set; }
    }

}
