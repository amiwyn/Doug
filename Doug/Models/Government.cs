using System;

namespace Doug.Models
{
    public class Government
    {
        public int Id { get; set; }
        public string Ruler { get; set; }
        public double TaxRate { get; set; }
        public string RevolutionLeader { get; set; }
        public string RevolutionTimestamp { get; set; }
        public DateTime RevolutionCooldown { get; set; }

        public bool IsInRevolutionCooldown()
        {
            return RevolutionCooldown > DateTime.UtcNow;
        }

        public int CalculateRevolutionCooldown()
        {
            return (int)(RevolutionCooldown - DateTime.UtcNow).TotalMinutes;
        }
    }
}
