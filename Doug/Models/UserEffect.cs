using System;
using System.ComponentModel.DataAnnotations.Schema;
using Doug.Effects;

namespace Doug.Models
{
    public class UserEffect
    {
        public string UserId { get; set; }
        public string EffectId { get; set; }
        public DateTime EndTime { get; set; }
        public User User { get; set; }
        [NotMapped]
        public Effect Effect { get; set; }

        public void CreateEffect(IEffectFactory effectFactory)
        {
            Effect = effectFactory.CreateEffect(EffectId);
        }
    }
}
