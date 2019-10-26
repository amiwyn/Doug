using Doug.Models.User;
using Doug.Slack;

namespace Doug.Effects.Buffs
{
    public class TrollBlessing : Buff
    {
        public const string EffectId = "troll_blessing";

        private readonly ISlackWebApi _slack;

        public TrollBlessing(ISlackWebApi slack)
        {
            _slack = slack;
            Id = EffectId;
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
