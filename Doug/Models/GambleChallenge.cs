using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doug.Models
{
    public class GambleChallenge
    {
        public int Id { get; set; }
        public string RequesterId { get; set; }
        public string TargetId { get; set; }
        public int Amount { get; set; }

        public GambleChallenge(string requesterId, string targetId, int amount)
        {
            RequesterId = requesterId;
            TargetId = targetId;
            Amount = amount;
        }
    }
}
