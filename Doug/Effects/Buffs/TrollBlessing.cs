using Doug.Models;
using Doug.Slack;

namespace Doug.Effects.Buffs
{
    public class TrollBlessing : Buff
    {
        private readonly ISlackWebApi _slack;

        public TrollBlessing(ISlackWebApi slack)
        {
            _slack = slack;
            Id = EffectFactory.TrollBlessing;
            Name = "Troll's Blessing";
            Description = "You are blessed by the gods. Immune to kicks. When someone kicks you, it kicks that person instead.";
            Rank = Rank.Divine;
            Icon = ":poop:";
        }

        public override bool OnKick(User kicker, string channel)
        {
            _slack.KickUser(kicker.Id, channel).Wait();
            return false;
        }
    }
}
